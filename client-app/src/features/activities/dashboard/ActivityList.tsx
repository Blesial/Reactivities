import { SyntheticEvent, useState } from "react";
import { Button, Item, Label, Segment } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";


export default observer(function ActivityList () {
    const {activityStore} = useStore();
    const {activitiesByDate, deleteActivity, loading} = activityStore;
    const [target, setTarget] = useState(''); // local state 

    // : Un evento sintético (SyntheticEvent) que se produce cuando se dispara un evento, en este caso, un evento de botón.
    function handleActivityDelete (e: SyntheticEvent<HTMLButtonElement>, id:string) {
        setTarget(e.currentTarget.name);
        deleteActivity(id);
    }   
 
    return (
        <Segment>
            <Item.Group divided>
                {activitiesByDate.map(activity => (
                    <Item key={activity.id}>
                        <Item.Content>
                            <Item.Header as='a'>{activity.title}</Item.Header>
                            <Item.Meta>{activity.date}</Item.Meta>
                            <Item.Description><div>{activity.description}</div><div>{activity.city}, {activity.venue}</div></Item.Description>
                            <Item.Extra>
                                <Button as={Link} to={`/activities/${activity.id}`} 
                                floated="right" 
                                content='View'
                                color="blue"/>
                                <Button 
                                onClick={(e) => handleActivityDelete(e, activity.id) } 
                                name={activity.id}  // le damos mas informacion a este boton para que se sepa cual es el que estamos clickeando y asi mostrar correctamente el loader unicamente en el boton que clickeamos
                                loading={loading && target === activity.id}  // Si submitting es true y target es igual a activity.id, entonces el botón estará en estado de carga (loading será true).
                                floated="right"
                                 content='Delete' 
                                 color="red"/>
                                <Label content={activity.category}/>
                            </Item.Extra>
                        </Item.Content>
                    </Item>
                ))}
                
            </Item.Group>
        </Segment>
    )
})