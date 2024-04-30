import { Col, Row } from 'react-bootstrap';
import Container from 'react-bootstrap/Container';
import 'react-toastify/dist/ReactToastify.css';
import MEDICALCENTER from '../../img/medicalcenter.png';
import './Home.css';

export const Home = () => {
    return (
        <>
            <Container className="home-container">
                <div className="home-header">
                    <Row>
                        <p>Welcome to our medical center</p>
                    </Row>
                    <Row>
                        <p>
                            You are invited to use the services of the best
                            medical center in our city and sign up for a
                            consultation with professionals
                        </p>
                    </Row>
                    <Row>
                        <Col>
                            <img
                                src={MEDICALCENTER}
                                width="500px"
                                height="300px"
                                alt="MEDICALCENTER"
                            />
                        </Col>
                    </Row>
                </div>
            </Container>
        </>
    );
};
