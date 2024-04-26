import React from 'react'
import { Navbar, Container } from 'react-bootstrap'
import Nav from 'react-bootstrap/Nav'

export class Header extends React.Component {
  render() {
    return (
      <Navbar className="nav-header">
        <Container>
          <Nav.Link className="nav-home">Medical center</Nav.Link>
          <Nav className="security-container" as="ul">
            <Nav.Item as="li">
              <Nav.Link className="nav-home">Register</Nav.Link>
            </Nav.Item>
            <Nav.Item as="li">
              <Nav.Link className="nav-home">Login</Nav.Link>
            </Nav.Item>
          </Nav>
        </Container>
      </Navbar>
    )
  }
}
