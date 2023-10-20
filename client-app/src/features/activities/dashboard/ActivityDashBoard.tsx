import { Grid } from "semantic-ui-react";
import ActivityList from "./ActivityList";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { useEffect } from "react";

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
export default observer(function ActivityDashBoard() {
  const { activityStore } = useStore();
  const {loadActivities, activityRegistry} = activityStore;

  useEffect(() => {
    if (activityRegistry.size <= 1) loadActivities();
  }, [loadActivities, activityRegistry]); // dependencies empty . cause to happend just once.

  // So what's going to happen if they're editing an activity and they click on cancel, then they're simply
  //going to go back to the activity details, open components.
  //And if they're just creating an activity, then all that's going to achieve is it's going to close the
  //form.
  if (activityStore.loadingInitial) return <LoadingComponent content="Loading App" />;

  return (
    <Grid>
      <Grid.Column width="10">
        <ActivityList />
      </Grid.Column>
      <Grid.Column width="6">
      <h2>Activities Filters</h2>
      </Grid.Column>
    </Grid>
  );
});
