export interface User {
    username: string,
    displayName: string,
    token: string,
    image?:string
};


// con este podemos usar la misma interfaz tanto para el login como para el register form. 
export interface UserFormValues {
    email: string,
    password: string,
    displayName?: string,
    userName?: string
};