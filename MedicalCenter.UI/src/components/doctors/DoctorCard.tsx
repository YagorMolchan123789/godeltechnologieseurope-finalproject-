import { Button, Card } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import apiConnector from '../../api/apiconnector';
import { DoctorDto } from '../../models/doctorDto';
import DoctorImage from './DoctorImage';
import './Doctor.css';

interface Props {
    doctor: DoctorDto;
    isShowButton: boolean;
}

function DoctorCard({ doctor, isShowButton }: Props) {
    return (
        <Card
            className="doctor-card"
            style={{ border: 'none', boxShadow: '0 4px 8px rgba(0,0,0,0.1)' }}
        >
            <DoctorImage
                imageNameWithType={doctor.photoUrl}
                style={{ maxWidth: '100%', height: 'auto' }}
            />
            <Card.Body className="doctor-card-body">
                <Card.Title className="doctor-card-title">
                    {doctor.firstName} {doctor.lastName}
                </Card.Title>
                <Card.Text>Specialization: {doctor.specialization}</Card.Text>
                <hr />
                <Card.Text>
                    {new Date().getFullYear() - doctor.practiceStartDate} years
                    of practice
                </Card.Text>
                <Link
                    state={{ doctor: doctor }}
                    to={'/../appointment/create/' + doctor.appUserId}
                >
                    <Button variant="primary">Choose time</Button>
                </Link>
                {isShowButton && (
                    <Button
                        className="mx-3"
                        variant="danger"
                        onClick={async () => {
                            await apiConnector.deleteDoctor(doctor.appUserId);
                            window.location.reload();
                        }}
                    >
                        Delete doctor
                    </Button>
                )}
            </Card.Body>
        </Card>
    );
}

export default DoctorCard;
