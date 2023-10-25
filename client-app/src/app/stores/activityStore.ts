import { makeAutoObservable, runInAction } from "mobx";
import { Activity } from "../models/activity";
import agent from "../api/agent";

export default class ActivityStore {
  // observables
  // activities: Activity[] = [];
  activityRegistry = new Map<string, Activity>(); // ta bueno el map object.
  selectedActivity: Activity | undefined = undefined;
  editMode = false;
  loading = false;
  loadingInitial = false;

  constructor() {
    makeAutoObservable(this);
  }

  get activitiesByDate() {
    return Array.from(this.activityRegistry.values()).sort(
      (a, b) => Date.parse(a.date) - Date.parse(b.date)
    );
  }

  // Verifica si ya existe una entrada en el objeto activities para la fecha date.
//Si existe, agrega la actividad actual activity al array existente de actividades para esa fecha, utilizando el operador ternario para crear un nuevo array que contiene todas las actividades previas y la nueva actividad.
// Si no existe una entrada para esa fecha, crea una nueva entrada en el objeto activities con la fecha como clave y un array que contiene solo la actividad actual.
// Este proceso se repite para cada actividad en activitiesByDate, lo que agrupa las actividades por fecha.
  get groupedActivities() {
    return Object.entries( //  Comienza con la creación de un array de pares clave-valor a partir de un objeto.
      this.activitiesByDate.reduce((activities, activity) => {// se utiliza para reducir un array a un solo valor acumulando información durante cada iteración. En este caso, se inicia con un objeto vacío llamado activities.
        const date = activity.date; // this is a string -> key of each object
        activities[date] = activities[date] ? [...activities[date], activity] : [activity]
        return activities;
      }, {} as {[key: string]: Activity[]})
    )
  }

  // action: usamos arrow function para bindear el this a la clase. sino tendriamos que especificar el bind dentro del constructor action.bound
  loadActivities = async () => {
    this.setLoadingInitial(true);
    try {
      const activities = await agent.Activities.list();
      runInAction(() => {
        // no podemos modificar un state del store luego de utilizar un await , necesitamos especificar que se sigue siendo un action
        // para eso wrappeamos todo lo que venga despues del uso del await.
        // otra forma mas facil es crear una action para el loadinginitial y llamarla dentro de esta action. y listo
        activities.forEach((a) => {
          this.setActivity(a);
        });
        this.loadingInitial = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loadingInitial = false;
      });
    }
  };

  setLoadingInitial = (state: boolean) => {
    this.loadingInitial = state;
  };

  loadActivity = async (id: string) => {
    let activity = this.getActivity(id);
    if (activity) {
      this.selectedActivity = activity;
      return activity;
    } else {
      this.setLoadingInitial(true);
      try {
        activity = await agent.Activities.details(id);
        this.setActivity(activity);
        runInAction(() => this.selectedActivity = activity);
        this.setLoadingInitial(false);
        return activity;
     
      } catch (error) {
        console.log(error);
      }
    }
  };

  private setActivity = (activity: Activity) => {
    activity.date = activity.date.split("T")[0];
    this.activityRegistry.set(activity.id, activity);
  };

  private getActivity = (id: string) => {
    return this.activityRegistry.get(id);
  };

  createActivity = async (activity: Activity) => {
    this.loading = true;
    try {
      await agent.Activities.create(activity);
      runInAction(() => {
        this.activityRegistry.set(activity.id, activity);
        this.selectedActivity = activity;
        this.editMode = false;
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  updateActivity = async (activity: Activity) => {
    this.loading = true;
    try {
      await agent.Activities.update(activity);
      runInAction(() => {
        this.activityRegistry.set(activity.id, activity);
        this.selectedActivity = activity;
        this.editMode = false;
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  deleteActivity = async (id: string) => {
    this.loading = true;
    try {
      await agent.Activities.delete(id);
      runInAction(() => {
        this.activityRegistry.delete(id);
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };
}
