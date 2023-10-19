import { Button, Container, Menu } from "semantic-ui-react";
import { useStore } from "../stores/store";
import { observer } from "mobx-react-lite";


export default observer(function NavBar () {
    const {activityStore} = useStore();

    return (
        <Menu inverted fixed='top'>
            <Container>
                <Menu.Item header>
                    <img src="/Images/logo.png" alt="logo" style={{marginRight: '10px'}}></img>
                    Reactivities
                </Menu.Item>
                <Menu.Item name="Activities"/>
                <Menu.Item>
                    <Button onClick={() => activityStore.openForm()} positive content='Create Activitie'/>
                </Menu.Item>
            </Container>
        </Menu>
    )
})