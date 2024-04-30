import React from 'react';
import { Container } from 'react-bootstrap';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import { NavLink } from 'react-router-dom';

export class Header extends React.Component {
    render() {
        return (
            <Navbar className="nav-header" bg="primary" data-bs-theme="dark">
                <Container>
                    <Navbar.Brand href="/">Medical center</Navbar.Brand>
                    <Nav className="me-auto">
                        <NavLink className="nav-link" to="/doctors">
                            Doctors
                        </NavLink>
                    </Nav>
                    <Nav className="security-container">
                        <Nav.Item>
                            <Nav.Link className="nav-home">Register</Nav.Link>
                        </Nav.Item>
                        <Nav.Item>
                            <Nav.Link className="nav-home">Login</Nav.Link>
                        </Nav.Item>
                    </Nav>
                </Container>
            </Navbar>
        );
    }
}
