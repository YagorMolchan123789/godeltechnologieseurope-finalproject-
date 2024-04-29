import Container from 'react-bootstrap/Container';
import { Outlet } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { Header } from './components/Header';
import './styles.css';

export const App = () => {
    return (
        <>
            <Header />
            <Container>
                <Outlet />
                <ToastContainer />
            </Container>
        </>
    );
};
