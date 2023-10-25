import { Button, Form, Segment } from "semantic-ui-react";
import { ChangeEvent, useEffect, useState } from "react";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import { Link, useNavigate, useParams } from "react-router-dom";
import { Activity } from "../../../app/models/activity";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import {v4 as uuid} from 'uuid';
import { Formik } from "formik";

// como tenemos dos variables (activity que viene de props, y estado local de este componente, con el mismo nombre, le damos un alias al activity pasado por props para hacer referencia)
export default observer(function ActivityForm () {
    // Our initial state is either going to be the selected activity that we're passing down here or it's
// going to be the properties that we would have inside an activity object.
    const {activityStore} = useStore();
    const {updateActivity, createActivity,
         loading, loadActivity, loadingInitial} = activityStore;

    const {id} = useParams();
    const navigate = useNavigate();

    // If the activity is null, then anything to the right of this is going to be used for the initial state.
    const [activity, setActivity]= useState<Activity>({ 
    id: '',
    title: '',
    category: '',
    description: '',
    date: '',
    city: '',
    venue: ''
});

    useEffect(() => {
        if (id) loadActivity(id).then(activity => setActivity(activity!));
    }, [id, loadActivity]);

    // function handleSubmit () {
    //     if (activity.id) {
    //         updateActivity(activity).then(() => navigate(`/activities/${activity.id}`))
    //      } else {
    //         activity.id = uuid();
    //         createActivity(activity).then(() => navigate(`/activities/${activity.id}`));
    //     } 
            
    // }

    // function handleChange   (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) {
    //     const {name, values} = e.target
    //     setActivity({...activity, [name]: values})
    // }

    if (loadingInitial) return <LoadingComponent content="Loading activity..."/>

    return ( 
        <Segment clearing>
            <Formik initialValues={activity} onSubmit={values => console.log(values)}>
                {({values, handleChange, handleSubmit}) => (
                     <Form onSubmit={handleSubmit} autoComplete='off'>
                     <Form.Input placeholder='Title' values={activity.title} name='title' onChange={handleChange}/>
                     <Form.TextArea placeholder='Description' values={activity.description} name='description' onChange={handleChange}/>
                     <Form.Input placeholder='Category' values={activity.category} name='category' onChange={handleChange}/>
                     <Form.Input type="date" placeholder='Date' values={activity.date} name='date' onChange={handleChange  }/>
                     <Form.Input placeholder='City' values={activity.city} name='city' onChange={handleChange  }/>
                     <Form.Input placeholder='Venue' values={activity.venue} name='venue' onChange={handleChange  }/>
                     <Button loading={loading} floated="right" positive type="submit" content='Submit'/>
                     <Button as={Link} to='/activities' floated="right" type="button" content='Cancel'/>
                     </Form>
                )}
            </Formik>
           
        </Segment>
    )
})