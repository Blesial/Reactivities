import { makeAutoObservable, runInAction } from "mobx";
import { Activity } from "../models/activity";
import agent from "../api/agent";
import { v4 as uuid } from "uuid";

export default class ActivityStore {
  // observables
  // activities: Activity[] = [];
  activityRegistry = new Map<string, Activity>() // ta bueno el map object. 
  selectedActivity: Activity | undefined = undefined;
  editMode = false;
  loading = false;
  loadingInitial = true;

  constructor() {
    makeAutoObservable(this);
  }

  get activitiesByDate () {
    return Array.from(this.activityRegistry.values()).sort((a, b) => 
    Date.parse(a.date) - Date.parse(b.date));
  }

  // action: usamos arrow function para bindear el this a la clase. sino tendriamos que especificar el bind dentro del constructor action.bound
  loadActivities = async () => {
    try {
      const activities = await agent.Activities.list();
      runInAction(() => {
        // no podemos modificar un state del store luego de utilizar un await , necesitamos especificar que se sigue siendo un action
        // para eso wrappeamos todo lo que venga despues del uso del await.
        // otra forma mas facil es crear una action para el loadinginitial y llamarla dentro de esta action. y listo
        activities.forEach(a => {
          a.date = a.date.split('T')[0];
          this.activityRegistry.set(a.id, a)
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

  selectActivity = async (id: string) => {
    this.selectedActivity = this.activityRegistry.get(id);
  };

  cancelSelectedActivity = () => {
    this.selectedActivity = undefined;
  };

  // si estamos editando nos va a pasar el id, si esta creando no necesitamos ningun id.
  openForm = (id?: string) => {
    // si esta creando uso el cancel por si antes de tocar la opcion de crear estaba editando otra activity.
    id ? this.selectActivity(id) : this.cancelSelectedActivity();
    this.editMode = true;
  };

  closeForm = () => {
    this.editMode = false;
  };

  createActivity = async (activity: Activity) => {
    this.loading = true;
    activity.id = uuid();
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
        if (this.selectedActivity?.id === id) this.cancelSelectedActivity();
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };
}
