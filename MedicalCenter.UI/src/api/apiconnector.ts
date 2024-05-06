import axios, { AxiosResponse } from 'axios';
import { getAllDoctorsDto } from '../models/getAllDoctorsDto';
const API_URL = process.env.REACT_APP_API_URL;

const apiConnector = {
    getAllDoctors: async (): Promise<getAllDoctorsDto> => {
        try {
            const accessToken = localStorage.getItem('accessToken');
            const url = API_URL + 'api/doctor';

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
};

export default apiConnector;
