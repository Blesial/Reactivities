import axios, { AxiosError, AxiosResponse } from "axios";
import { Activity } from "../models/activity";
import { toast } from "react-toastify";
import { router } from "../router/routes";
import { store } from "../stores/store";
import { User, UserFormValues } from "../models/user";

// vamos a crear un delay en la carga de la pagina para hacerlo mas realista:

const sleep = (delay: number) => {
  return new Promise((resolve) => {
    setTimeout(resolve, delay);
  });
};

// vamos a centralizar los api requests en este archivo. (todos los metodos dentro de app.tsx requieren utilizar axios)

// base url :
axios.defaults.baseURL = "http://localhost:5000/api";

// axios intercepters :
axios.interceptors.response.use(
  async (response) => {
    await sleep(1000);
    return response;
  },
  (error: AxiosError) => {
    // config para ver que lo q recibimos es un get request (para el caso en que la peticion no envie un guid)
    const {data, status, config} = error.response as AxiosResponse;// cuando accedemos al data, no tendremos typesafety pero con el as axiosreponse sabemos que ese objeto data existe

    switch(status) {
        case 400:
          // para mandar a la persona que hizo un rquest de una activity q devuelve un 400, es porque envio mal el id. 
              if (config.method === 'get' && data.errors.hasOwnProperty('id')) {
                router.navigate('/not-found')
              }
            if (data.errors) {
                const modalStateErrors = [];
                for (const key in data.errors) {
                    if (data.errors[key]) {
                        modalStateErrors.push(data.errors[key]);
                    }
                }
                throw modalStateErrors.flat();
                // if we get a normal 400
            } else {
                toast.error(data);
            }
            break;
        case 401: 
            toast.error("Unauthorized");
            break;
        case 403:
            toast.error("Forbidden");
            break;
        case 404:
            router.navigate('/not-found');
            break;
        case 500:
            store.commonStore.setServerError(data);
            router.navigate('/server-error');
            break;

    }
    return Promise.reject(error);
  }
);

// Agregamos un tipo generico para nuestro responsebody para conseguir type safety:
// entonces en el caso de list: el T seria sustituido por el activity[]
const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
  //
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  post: <T>(url: string, body: {}) =>
    axios.post<T>(url, body).then(responseBody),
  put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
  del: <T>(url: string) => axios.delete<T>(url).then(responseBody),
};

const Activities = {
  // hay que especificar el tipo de dato que retorna este get: pero hay que modificar el reponse body
  list: () => requests.get<Activity[]>("/activities"),
  details: (id: string) => requests.get<Activity>(`/activities/${id}`),
  create: (activity: Activity) => requests.post<void>("/activities", activity),
  update: (activity: Activity) =>
    requests.put<void>(`/activities/${activity.id}`, activity),
  delete: (id: string) => requests.del<void>(`/activities/${id}`),

  // vemos como el metodo list devuelve un Promise del tipo Activity[]. lo logramos con <T> : Tipos genericos.
};

// metodos para el usuario, logearlo, registrarlo, etc.
const Account = {
  current: () => requests.get<User>("/account"),
  login: (user: UserFormValues) => requests.post<User>('/account/login', user),
  register: (user: UserFormValues) => requests.post<User>('account/register', user) 
};

const agent = {
  Activities,
  Account
};

export default agent;
