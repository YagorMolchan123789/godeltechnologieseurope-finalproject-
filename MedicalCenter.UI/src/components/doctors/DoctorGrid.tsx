import { useState, useEffect } from 'react';
import Container from 'react-bootstrap/Container';
import apiConnector from '../../api/apiconnector';
import { Col, Row } from 'react-bootstrap';
import DoctorCard from './DoctorCard';
import { getAllDoctorsDto } from '../../models/getAllDoctorsDto';
import { DoctorDto } from '../../models/doctorDto';

export default function DoctorGrid() {
    const [data, setData] = useState<getAllDoctorsDto>({
        doctorInfos: [],
        isShowButton: false,
    });

    useEffect(() => {
        const fetchData = async () => {
            const response = await apiConnector.getAllDoctors();

            setData({
                doctorInfos: response.doctorInfos,
                isShowButton: response.isShowButton,
            });
        };

        fetchData();
    }, []);

    return (
        <Container>
            <h1 className="doctors-page-title">Our doctors</h1>
            <Row
                xs={1}
                md={2}
                lg={3}
                className="justify-content-center doctor-row-container"
            >
                {data.doctorInfos.length !== 0 &&
                    data.doctorInfos.map((doctor: DoctorDto, index: number) => (
                        <Col key={doctor.appUserId} className="doctor-col">
                            <DoctorCard
                                key={index}
                                doctor={doctor}
                                isAdmin={data.isShowButton}
                            />
                        </Col>
                    ))}
            </Row>
        </Container>
    );
}
