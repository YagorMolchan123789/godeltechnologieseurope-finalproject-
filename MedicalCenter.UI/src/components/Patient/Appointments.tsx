import { useEffect, useState } from 'react';
import { Row, Tab, Tabs } from 'react-bootstrap';
import Container from 'react-bootstrap/Container';
import apiConnector from '../../api/apiconnector';
import { userAppointment } from '../../models/userAppointment';
import AppointmentCard from './AppointmentCard';

export default function UserAppointments() {
    const [appointments, setAppointments] = useState<userAppointment[]>([]);

    useEffect(() => {
        const fetchData = async () => {
            const appointments = await apiConnector.getUserAppointments();
            setAppointments(appointments);
        };

        fetchData();
    }, []);

    const onCancelHandler = async (id: number) => {
        try {
            await apiConnector.cancelAppointment(id);
            setAppointments(appointments.filter((a) => a.id !== id));
        } catch (error: unknown) {
            console.error('Failed to cancel appointment:', error);
        }
    };

    const renderContent = () => {
        return (
            <Row className="justify-content-center">
                <Tabs
                    defaultActiveKey="upcoming"
                    id="uncontrolled-tab-example"
                    className="mb-3 col-md-4 justify-content-center"
                    justify
                >
                    <Tab eventKey="upcoming" title="Upcoming">
                        <Row className="justify-content-center">
                            {appointments
                                .filter((appointment) => !appointment.isPast)
                                .map((appointment) => (
                                    <Row
                                        key={appointment.id}
                                        md={2}
                                        className="justify-content-center mb-3"
                                    >
                                        <AppointmentCard
                                            key={appointment.id}
                                            data={appointment}
                                            onCancel={() =>
                                                onCancelHandler(appointment.id)
                                            }
                                        />
                                    </Row>
                                ))}
                        </Row>
                    </Tab>
                    <Tab eventKey="past" title="Past">
                        <Row className="justify-content-center">
                            {appointments
                                .filter((appointment) => appointment.isPast)
                                .map((appointment) => (
                                    <Row
                                        key={appointment.id}
                                        md={2}
                                        className="justify-content-center mb-3"
                                    >
                                        <AppointmentCard
                                            key={appointment.id}
                                            data={appointment}
                                            onCancel={() =>
                                                onCancelHandler(appointment.id)
                                            }
                                        />
                                    </Row>
                                ))}
                        </Row>
                    </Tab>
                </Tabs>
            </Row>
        );
    };

    return (
        <Container className="justify-content-center">
            <h1 className="text-center">
                {appointments.length == 0
                    ? "You don't have any appointments"
                    : 'Appointments'}
            </h1>
            {appointments.length !== 0 && renderContent()}
        </Container>
    );
}
