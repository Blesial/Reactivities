import { makeAutoObservable } from "mobx";
import { ServerError } from "../models/serverError";

export default class commonStore {
    error: ServerError | null = null;
    token: string | null | undefined = null;
    appLoaded = false;

    constructor() {
        makeAutoObservable(this);
    }

    setServerError (error: ServerError) {
        this.error = error;
    }

    setToken = (token: string | null | undefined) => {
        // key value pair
        if (token) localStorage.setItem('jwt', token);
        this.token = token;
    }

    setAppLoaded = () => {
        this.appLoaded = true;
    }
}