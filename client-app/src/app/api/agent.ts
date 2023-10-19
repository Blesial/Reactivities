import axios, { AxiosResponse } from 'axios';
import { Activity } from '../models/activity';

// vamos a crear un delay en la carga de la pagina para hacerlo mas realista:

const sleep = (delay: number) => {
    return new Promise((resolve) => {
        setTimeout(resolve, delay) 
    })
};



// vamos a centralizar los api requests en este archivo. (todos los metodos dentro de app.tsx requieren utilizar axios)

// base url :
    axios.defaults.baseURL = 'http://localhost:5000/api';

// axios intercepters : 
    axios.interceptors.response.use(async response => {
        try {
            await sleep(1000);
            return response;
        } catch (error) {
            console.log(error);
            return await Promise.reject(error);
        }
    })

    // Agregamos un tipo generico para nuestro responsebody para conseguir type safety:
    // entonces en el caso de list: el T seria sustituido por el activity[] 
    const responseBody = <T> (response:AxiosResponse<T>) => response.data;  

    const requests = {
        // 
        get: <T> (url: string) => axios.get<T>(url).then(responseBody),
        post: <T> (url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
        put: <T> (url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
        del: <T> (url: string) => axios.delete<T>(url).then(responseBody),
    };

    const Activities = {
        // hay que especificar el tipo de dato que retorna este get: pero hay que modificar el reponse body
        list: () => requests.get<Activity[]>('/activities'),
        details: (id:string) => requests.get<Activity>(`/activities/${id}`),
        create: (activity: Activity) => requests.post<void>('/activities', activity),
        update: (activity: Activity) => requests.put<void>(`/activities/${activity.id}`, activity),
        delete: (id: string) => requests.del<void>(`/activities/${id}`),
        
        // vemos como el metodo list devuelve un Promise del tipo Activity[]. lo logramos con <T> : Tipos genericos. 
    };

    const agent = {
        Activities
    };

    export default agent;