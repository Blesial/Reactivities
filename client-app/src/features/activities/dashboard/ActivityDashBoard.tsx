import { Grid } from "semantic-ui-react";
import { Activity } from "../../../app/models/activity";
import ActivityList from "./ActivityList";
import ActivityDetails from "../details/ActivitiesDetails";
import ActivityForm from "../form/ActivityForm";

interface Props {
  activities: Activity[];
  selectedActivity: Activity | undefined;
  selectActivity: (id: string) => void;
  cancelSelectedActivity: () => void;
  editMode: boolean;
  openForm: (id: string) => void; // we don't need to make this optional because when we're using this inside our dashboard, we're actually passing this down to our activity details component.And we'll always have an ID, So this one is not going to be optional.
  closeForm: () => void;
  createOrEdit: (activity: Activity) => void;
  deleteActivity: (id: string) => void;
}

// semantic ui grid system para hacer el layout el contenido en nuestras paginas.
// css frameworks todos vienen con grid sistems como bootstrap
// grid system" en el contexto de los frameworks de CSS es una estructura predefinida
// y reutilizable que se utiliza para diseñar la disposición de elementos en una página web
// de una manera organizada y consistente
//  sistema de cuadrícula

// BOOTSTRAP TIENE 12 COLUMN GRID
// SEMANTIC UI TIENE 16 !!!

// CDN (Content Delivery Network) que es ? ->  Un CDN (Content Delivery Network) es una red de servidores distribuidos geográficamente que se utiliza para entregar contenido web, como archivos estáticos (imágenes, hojas de estilo, scripts, videos, etc.), de manera más eficiente y rápida a los usuarios finales

// Bootstrap ofrece clases adicionales para modificar la apariencia y el comportamiento de las columnas
// , como ajustar los márgenes (mx-auto), ocultar columnas en ciertos tamaños de pantalla (d-none, d-md-block), y más.
export default function ActivityDashBoard({
  activities,
  selectActivity,
  selectedActivity,
  cancelSelectedActivity,
  editMode,
  openForm,
  closeForm,
  createOrEdit,
  deleteActivity
}: Props) {
  return (
    <Grid>
      <Grid.Column width="10">
        <ActivityList activities={activities} selectActivity={selectActivity} deleteActivity={deleteActivity} />
      </Grid.Column>
      <Grid.Column width="6">
        {selectedActivity && !editMode && (
          <ActivityDetails
            activity={selectedActivity}
            cancelSelectActivity={() => cancelSelectedActivity()}
            openForm={openForm}

          />
        )}
        {editMode &&
        <ActivityForm closeForm={closeForm} activity={selectedActivity} createOrEdit={createOrEdit}/>}
      </Grid.Column>
    </Grid>
  );
}
