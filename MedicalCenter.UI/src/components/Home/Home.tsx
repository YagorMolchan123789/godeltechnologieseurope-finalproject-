import { Button, Col, Row } from 'react-bootstrap';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Container from 'react-bootstrap/Container';
import MEDICALCENTER from '../../public/images/medicalcenter.png';
import 'react-toastify/dist/ReactToastify.css';
import './Home.css';

export const Home = () => {
    const navigate = useNavigate();
    const accessTokenKey: string = 'accessToken';
    const [isAuthenticaticated, setIsAuthenticated] = useState(false);

    const calltoAction = () => {
        isAuthenticaticated ? navigate('/doctors') : navigate('/login');
    };

    useEffect(() => {
        const accessToken: string | null = localStorage.getItem(accessTokenKey);
        if (accessToken) {
            setIsAuthenticated(true);
        } else {
            setIsAuthenticated(false);
        }
    });

    return (
        <>
            <Container
                id="home"
                className="home-container d-flex justify-content-center align-items-center vh-100"
            >
                <img
                    src={MEDICALCENTER}
                    className="home-image"
                    alt="MEDICALCENTER"
                />
                <Col className="home-content bg-white p-4 rounded">
                    <Row className="mb-3">
                        <h1 className="title mx-auto">Medical Center</h1>
                    </Row>
                    <Row className="mb-3">
                        <p>
                            Welcome! Find the right across a range of popular
                            book appointments, and manage your visits
                            seamlessly. Access your appointment history, review
                            past consultations, and take charge of your
                            healthcare journey.
                            <br /> <br /> Empowering you with convenient tools
                            healthier tomorrow.
                        </p>
                    </Row>
                    <Row>
                        <Button className="btn-home" onClick={calltoAction}>
                            Call to action
                        </Button>
                    </Row>
                </Col>
            </Container>
        </>
    );
};
