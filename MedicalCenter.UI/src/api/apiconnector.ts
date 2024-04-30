import { DoctorDto } from '../models/doctorDto';
const API_URL = process.env.REACT_APP_API_URL;

const apiConnector = {
    getAllDoctors: async (): Promise<DoctorDto[]> => {
        try {
            const response = await fetch(API_URL + 'api/doctors', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (!response.ok) {
                throw new Error(
                    `Error: ${response.status} ${response.statusText}`
                );
            }

            const data: DoctorDto[] = await response.json();
            return data;
        } catch (error: unknown) {
            console.error('Failed to fetch doctors:', error);
            return [];
        }
    },
};

export default apiConnector;
