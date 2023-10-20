import "./styles.css";
import { Container } from "semantic-ui-react";
import NavBar from "./NavBar";
// algunos paquetes que instalemos no vienen escritos para ser leidos por typescript y solo por javascript
// pero se soluciona: buscar typescript definition file outthere for uuid.
import { observer } from "mobx-react-lite";
import { Outlet, useLocation } from "react-router-dom";
import HomePage from "../../features/home/HomePage";

//MOBX: Deriving State: Deriving state means calculating or computing values from the existing
// application state. For instance, in a shopping cart application,
// the total cost of items in the cart can be derived from the individual prices and quantities of items.

// Automatically: MobX uses a concept known as reactive programming.
//  It tracks dependencies between state and the values derived from that state.
//  When any part of the state changes, MobX automatically updates all the derived values that depend on it.

function App() {
  // aca no pinta usar el use effect para traer las activities -> hay q pasarlo al activity dashboard,
  const location = useLocation(); // nos da el path

  return (
    <>
      {location.pathname === "/" ? <HomePage /> : (
        <>
          <NavBar />
          <Container style={{ marginTop: "7em" }}>
            {/* renderiza cualquier hijo de app al que se este accediendo */}
            <Outlet />
          </Container>
        </>
      )}
    </>
  );
}
export default observer(App);
