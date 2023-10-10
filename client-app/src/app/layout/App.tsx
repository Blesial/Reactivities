import { useEffect, useState } from 'react';
import './styles.css';
import axios from 'axios';
import { Activity } from '../models/activity';
import { Container } from 'semantic-ui-react';
import NavBar from './NavBar';
import ActivityDashBoard from '../../features/activities/dashboard/ActivityDashBoard';
// algunos paquetes que instalemos no vienen escritos para ser leidos por typescript y solo por javascript
// pero se soluciona: buscar typescript definition file outthere for uuid. 
import {v4 as uuid} from 'uuid';

function App() {
const [activities, setActivities] = useState<Activity[]>([]);
const [selectedActivity, setSelectedActivity] = useState<Activity | undefined>(undefined);
const [editMode, setEditMode] = useState(false); // want to display it when we're either editing or we're creating an activity

useEffect(() => {
  axios.get<Activity[]>('http://localhost:5000/api/Activities')
    .then(response => {
      console.log(response);
      setActivities(response.data);
    })
}, []); // dependencies empty . cause to happend just once. 

function handleSelectedActivity(id: string) {
  setSelectedActivity(activities.find(x => x.id === id)); //Metodo que regresa el primer objeto del array que cumple con la condicion. 
}

function handleCancelSelectedActivity ()   {
  setSelectedActivity(undefined);
}

function handleFormOpen (id?: string)   { // optional id parameter
    id ? handleSelectedActivity(id) : handleCancelSelectedActivity();
    setEditMode(true);
}

function handleFormClose ()   {
  setEditMode(false);
}

// Para funcionalidad del Submit del Form 
function handleCreateOrEditActivity (activity: Activity) {
  // si el activity id existe quiere decir que estamos editando una actividad existente
  activity.id ? setActivities([...activities.filter(x => x.id !== activity.id), activity]) 
              : setActivities([...activities, {...activity, id: uuid()}]);

  setEditMode(false); // para que se cierre el editor
  setSelectedActivity(activity); // para que se actualice los detalles de la actividad editadas recientemente

}

function handleDelete (id: string) {
  setActivities([...activities.filter(x => x.id !== id)])
}

// So what's going to happen if they're editing an activity and they click on cancel, then they're simply
//going to go back to the activity details, open components.
//And if they're just creating an activity, then all that's going to achieve is it's going to close the
//form.


  return (
    <>
      <NavBar openForm={handleFormOpen} />
      <Container style={{marginTop:'7em'}}>
       <ActivityDashBoard 
       activities={activities}
       selectedActivity={selectedActivity}
       selectActivity={handleSelectedActivity}
       cancelSelectedActivity={handleCancelSelectedActivity}
       editMode ={editMode}
       openForm ={handleFormOpen}
       closeForm ={handleFormClose}
       createOrEdit={handleCreateOrEditActivity}
       deleteActivity={handleDelete}
       />
        </Container>
        </>
  );
}

export default App;
