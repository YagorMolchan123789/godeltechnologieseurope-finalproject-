import React, { useState } from 'react';
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import './Header.css';
import { Link } from 'react-router-dom';

export default function Header() {
    const accessTokenKey: string = 'accessToken';
    const visibilityStyleNone = { display: 'none' };
    const visibilityStyleBlock = { display: 'block' };

    const [visibilityUserOptions, setVisibilityUserOptions] =
        useState(visibilityStyleNone);
    const [visibilityRegisterAndLogin, setVisibilityRegisterAndLogin] =
        useState(visibilityStyleBlock);

    setInterval(checkToken, 100);

    function checkToken() {
        const accessToken: string | null = localStorage.getItem(accessTokenKey);

        if (accessToken) {
            setVisibilityUserOptions(visibilityStyleBlock);
            setVisibilityRegisterAndLogin(visibilityStyleNone);
        } else {
            setVisibilityUserOptions(visibilityStyleNone);
            setVisibilityRegisterAndLogin(visibilityStyleBlock);
        }
    }

    return (
        <Navbar className="nav-header">
            <Container>
                <Link className="nav-home" to="/">
                    Medical center
                </Link>
                <Nav className="security-container" as="ul">
                    <Nav style={visibilityRegisterAndLogin}>
                        <Nav.Item as="li">
                            <Link className="nav-home" to="/register">
                                Register
                            </Link>
                        </Nav.Item>
                        <Nav.Item as="li">
                            <Link className="nav-home" to="/login">
                                Login
                            </Link>
                        </Nav.Item>
                    </Nav>
                    <Nav style={visibilityUserOptions}>
                        <Nav.Item as="li">
                            <Link className="nav-home" to="/profile">
                                User profile
                            </Link>
                        </Nav.Item>
                        <Nav.Item as="li">
                            <Link className="nav-home" to="/doctors">
                                Choose Doctor
                            </Link>
                        </Nav.Item>
                    </Nav>
                </Nav>
            </Container>
        </Navbar>
    );
}
