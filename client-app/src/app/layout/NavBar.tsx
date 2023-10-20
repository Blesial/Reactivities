import { Button, Container, Menu } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { NavLink } from "react-router-dom";


export default observer(function NavBar () {

    return (
        <Menu inverted fixed='top'>
            <Container>
                <Menu.Item as={NavLink} to='/'     header>
                    <img src="/Images/logo.png" alt="logo" style={{marginRight: '10px'}}></img>
                    Reactivities
                </Menu.Item>
                <Menu.Item as={NavLink} to='/activities' name="Activities"/>
                <Menu.Item>
                    <Button as={NavLink} to='/createActivity' positive content='Create Activitie'/>
                </Menu.Item>
            </Container>
        </Menu>
    )
})