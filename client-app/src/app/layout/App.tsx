import { useEffect } from "react";
import "./styles.css";
import { Container } from "semantic-ui-react";
import NavBar from "./NavBar";
import ActivityDashBoard from "../../features/activities/dashboard/ActivityDashBoard";
// algunos paquetes que instalemos no vienen escritos para ser leidos por typescript y solo por javascript
// pero se soluciona: buscar typescript definition file outthere for uuid.
import LoadingComponent from "./LoadingComponent";
import { useStore } from "../stores/store";
import { observer } from "mobx-react-lite";

//MOBX: Deriving State: Deriving state means calculating or computing values from the existing
// application state. For instance, in a shopping cart application,
// the total cost of items in the cart can be derived from the individual prices and quantities of items.

// Automatically: MobX uses a concept known as reactive programming.
//  It tracks dependencies between state and the values derived from that state.
//  When any part of the state changes, MobX automatically updates all the derived values that depend on it.

function App() {
  const { activityStore } = useStore();

  useEffect(() => {
    activityStore.loadActivities();
  }, [activityStore]); // dependencies empty . cause to happend just once.

  // So what's going to happen if they're editing an activity and they click on cancel, then they're simply
  //going to go back to the activity details, open components.
  //And if they're just creating an activity, then all that's going to achieve is it's going to close the
  //form.
  if (activityStore.loadingInitial)
    return <LoadingComponent content="Loading App" />;
  return (
    <>
      <NavBar />
      <Container style={{ marginTop: "7em" }}>
        <ActivityDashBoard />
      </Container>
    </>
  );
}
export default observer(App);
