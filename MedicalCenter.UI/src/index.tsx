import { App } from './App';
import { createRoot } from 'react-dom/client';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { AppointmentTimeForm } from './components/AppointmentTimeForm';
import { Home } from './components/Home';
import ErrorPage from './components/ErrorPage/ErrorPage';
import DoctorGrid from './components/doctors/DoctorGrid';

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
        ],
    },
]);

const container = document.getElementById('root');
const root = createRoot(container!);
root.render(<RouterProvider router={router} />);
