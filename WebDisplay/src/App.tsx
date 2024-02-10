import './App.scss';

import { Routes, Route, Link } from 'react-router-dom';
import { Container, Nav, NavDropdown, Navbar } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBars } from '@fortawesome/free-solid-svg-icons';

import Dashboard from './views/dashboard/main';

function App() {
    return (
        <>
            <Navbar variant="dark" bg="primary">
                <Container fluid>
                    <Navbar.Brand as={Link} to="/">
                        Home Monitor
                    </Navbar.Brand>
                    <Nav>
                        <NavDropdown
                            title={
                                <FontAwesomeIcon
                                    width={32}
                                    height={32}
                                    icon={faBars}
                                />
                            }
                            align="end"
                        >
                            <NavDropdown.Item as={Link} to="/summary">
                                Summary
                            </NavDropdown.Item>
                        </NavDropdown>
                    </Nav>
                </Container>
            </Navbar>
            <Routes>
                <Route path="/" element={<Dashboard />} />
            </Routes>
        </>
    );
}

export default App;
