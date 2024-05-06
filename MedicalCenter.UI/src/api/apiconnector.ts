import axios, { AxiosResponse } from 'axios';
import { getAllDoctorsDto } from '../models/getAllDoctorsDto';
import { userAppointment } from '../models/userAppointment';
const API_URL = process.env.REACT_APP_API_URL;

const apiConnector = {
    getAllDoctors: async (): Promise<getAllDoctorsDto> => {
        try {
            const accessToken = localStorage.getItem('accessToken');
            const url = API_URL + 'api/doctor/doctors';

            const api = axios.create({
                headers: {
                    Authorization: accessToken,
                    'Access-Control-Allow-Headers': '*',
                    'Access-Control-Allow-Origin': '*',
                    'Access-Control-Allow-Methods': '*',
                    Allow: '*',
                },
            });

            const response: AxiosResponse<getAllDoctorsDto> =
                await api.get(url);

            return response.data as getAllDoctorsDto;
        } catch (error: unknown) {
            console.error('Failed to fetch doctors:', error);
            return {
                doctorInfos: [],
                isShowButton: false,
            };
        }
    },

    deleteDoctor: async (doctorId: string): Promise<void> => {
        const accessToken = localStorage.getItem('accessToken');
        const url = API_URL + `api/doctor/${doctorId}`;

        await axios.delete<string>(url, {
            headers: {
                Authorization: accessToken,
                'Access-Control-Allow-Headers': '*',
                'Access-Control-Allow-Origin': '*',
                'Access-Control-Allow-Methods': '*',
                Allow: '*',
            },
        });
    },
    getUserAppointments: async (): Promise<userAppointment[]> => {
        try {
            const accessToken = localStorage.getItem('accessToken');
            const { data } = await axios.get<userAppointment[]>(
                API_URL + 'api/appointment',
                {
                    headers: {
                        Authorization: accessToken,
                        'Access-Control-Allow-Headers': '*',
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': '*',
                        Allow: '*',
                    },
                }
            );
            return data;
        } catch (error: unknown) {
            console.error('Failed to get appointments:', error);
            return [];
        }
    },
    cancelAppointment: async (id: number) => {
        try {
            const accessToken = localStorage.getItem('accessToken');
            await axios.delete(API_URL + 'api/appointment', {
                params: { id },
                headers: {
                    Authorization: accessToken,
                    'Access-Control-Allow-Headers': '*',
                    'Access-Control-Allow-Origin': '*',
                    'Access-Control-Allow-Methods': '*',
                    Allow: '*',
                },
            });
        } catch (error: unknown) {
            console.error('Failed to cancel appointment:', error);
        }
    },
};

export default apiConnector;
