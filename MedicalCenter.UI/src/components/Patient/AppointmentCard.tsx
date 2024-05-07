import { Button, Col, Row } from 'react-bootstrap';
import { userAppointment } from '../../models/userAppointment';
import './Appointment.css';

interface Props {
    data: userAppointment;
    onCancel: () => void;
}

const removeSeconds = (timeString: string): string => {
    const [hours, minutes] = timeString.split(':');
    return `${hours}:${minutes}`;
};

function AppointmentCard({ data, onCancel }: Props) {
    return (
        <Row xs={1} md={2} lg={2} className="appointment-item">
            <Col>
                <h3>
                    {data.doctorInfo.firstName} {data.doctorInfo.lastName}
                </h3>
                <p>Specialization: {data.doctorInfo.specialization}</p>
                <p>
                    {new Date().getFullYear() -
                        data.doctorInfo.practiceStartDate}{' '}
                    years of practice
                </p>
            </Col>
            <Col className="appointment-cancel">
                <p>
                    Date: {data.date} {removeSeconds(data.time)}
                </p>
                {!data.isPast && (
                    <Button variant="danger" onClick={onCancel}>
                        Cancel
                    </Button>
                )}
            </Col>
        </Row>
    );
}

export default AppointmentCard;
