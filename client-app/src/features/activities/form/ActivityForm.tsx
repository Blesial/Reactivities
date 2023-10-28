import { Button, FormField, Label, Segment } from "semantic-ui-react";
import { ChangeEvent, useEffect, useState } from "react";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import { Link, useNavigate, useParams } from "react-router-dom";
import { Activity } from "../../../app/models/activity";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import {v4 as uuid} from 'uuid';
import { Formik, Form, Field, ErrorMessage } from "formik";
import * as yup from 'yup';

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

// este objeto va a tomar las propiedades de nuestro form para validarse contra ellas
    const validationSchema = yup.object({
        title: yup.string().required('The activity title is required'),
    })

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
    //     const {name, value} = e.target
    //     setActivity({...activity, [name]: value})
    // }

    if (loadingInitial) return <LoadingComponent content="Loading activity..."/>

    return ( 
        <Segment clearing>
            <Formik 
                    validationSchema={validationSchema}
                    enableReinitialize 
                    initialValues={activity} 
                    onSubmit={values => console.log(values)}>
                {({handleSubmit}) => (
                     <Form className="ui form" onSubmit={handleSubmit} autoComplete='off'>
                        <FormField>
                        <Field placeholder='Title' name='title'/>
                        <ErrorMessage name="title" render={error => <Label basic color="red" content={error}/>}/>
                        </FormField>
                     <Field placeholder='Description' name='description'/>
                     <Field placeholder='Category' name='category'/>
                     <Field type="date" placeholder='Date' name='date'/>
                     <Field placeholder='City' name='city'/>
                     <Field placeholder='Venue' name='venue'/>
                     <Button loading={loading} floated="right" positive type="submit" content='Submit'/>
                     <Button as={Link} to='/activities' floated="right" type="button" content='Cancel'/>
                     </Form>
                )}
            </Formik>
           
        </Segment>
    )
})