import { App } from './App';
import { createRoot } from 'react-dom/client';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { AppointmentTimeForm } from './components/AppointmentTimeForm/AppointmentTimeForm';
import { Home } from './components/Home/Home';
import ErrorPage from './components/ErrorPage/ErrorPage';
import DoctorGrid from './components/doctors/DoctorGrid';
import Login from './components/Login/Login';
import Register from './components/Register/Register';
import RegisterDoctor from './components/RegisterDoctor/RegisterDoctor';
import 'bootstrap/dist/css/bootstrap.min.css';
import './global.css';

const router = createBrowserRouter([
    {
        path: '/',
        element: <App />,
        errorElement: <ErrorPage />,
        children: [
            {
                path: '/',
                element: <Home />,
            },
            {
                path: 'appointment/create/:doctorId',
                element: <AppointmentTimeForm />,
            },
            {
                path: 'doctors',
                element: <DoctorGrid />,
            },
            {
                path: '/login',
                element: <Login />,
            },
            {
                path: '/register',
                element: <Register />,
            },
            {
                path: '/register-doctor',
                element: <RegisterDoctor />,
            },
        ],
    },
]);

const container = document.getElementById('root');
const root = createRoot(container!);
root.render(<RouterProvider router={router} />);
