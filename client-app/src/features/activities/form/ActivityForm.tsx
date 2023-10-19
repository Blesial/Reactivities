import { Button, Form, Segment } from "semantic-ui-react";
import { ChangeEvent, useState } from "react";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";

// como tenemos dos variables (activity que viene de props, y estado local de este componente, con el mismo nombre, le damos un alias al activity pasado por props para hacer referencia)
export default observer(function ActivityForm () {
    // Our initial state is either going to be the selected activity that we're passing down here or it's
// going to be the properties that we would have inside an activity object.
    const {activityStore} = useStore();
    const {closeForm, selectedActivity, updateActivity, createActivity, loading} = activityStore;

    // If the activity is null, then anything to the right of this is going to be used for the initial state.
    const initialState = selectedActivity ?? {
        id: '',
        title: '',
        category: '',
        description: '',
        date: '',
        city: '',
        venue: ''
    }

    const [activity, setActivity]= useState(initialState);


    function handleSubmit () {
        activity.id ? updateActivity(activity) : createActivity(activity);
    }

    function handleInputChange (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) {
        const {name, value} = e.target
        setActivity({...activity, [name]: value})
    }

    return ( 
        <Segment clearing>
            <Form onSubmit={handleSubmit} autoComplete='off'>
            <Form.Input placeholder='Title' value={activity.title} name='title' onChange={handleInputChange}/>
            <Form.TextArea placeholder='Description' value={activity.description} name='description' onChange={handleInputChange}/>
            <Form.Input placeholder='Category' value={activity.category} name='category' onChange={handleInputChange}/>
            <Form.Input type="date" placeholder='Date' value={activity.date} name='date' onChange={handleInputChange}/>
            <Form.Input placeholder='City' value={activity.city} name='city' onChange={handleInputChange}/>
            <Form.Input placeholder='Venue' value={activity.venue} name='venue' onChange={handleInputChange}/>
            <Button loading={loading} floated="right" positive type="submit" content='Submit'/>
            <Button  onClick={closeForm} floated="right" type="button" content='Cancel'/>
            </Form>
        </Segment>
    )
})