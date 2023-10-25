import { Navigate, RouteObject, createBrowserRouter } from "react-router-dom";
import App from "../layout/App";
import ActivityDashBoard from "../../features/activities/dashboard/ActivityDashBoard";
import ActivityForm from "../../features/activities/form/ActivityForm";
import ActivitiesDetails from "../../features/activities/details/ActivitiesDetails";
import TestErrors from "../../features/errors/TestError";
import NotFound from "../../features/errors/NotFound";
import ServerError from "../../features/errors/ServerError";

export const routes: RouteObject[] = [
    { // root route : app 
        path: '/',
        element: <App/>,
        // root childrens 
        children: [
            {path: 'activities', element: <ActivityDashBoard/>},
            {path: 'activities/:id', element: <ActivitiesDetails/>},
            {path: 'createActivity', element: <ActivityForm key='create'/>},
            // React por default preserva el mismo estado para componentes q matienen su misma posicion,
            // pero nosotros podemos modificar o controlarlo reseteando el estado manualmente para que no se preserve el estado por default
            // para ello utilizamos un Component KEY !
            {path: 'manage/:id', element: <ActivityForm key='manage'/>},
            {path: 'errors', element: <TestErrors/>},
            {path: 'not-found', element: <NotFound/>},
            {path: '/server-error', element: <ServerError/>},
            {path: '*', element: <Navigate replace to='/not-found'/>},
        ]
    }
];

export const router = createBrowserRouter(routes)