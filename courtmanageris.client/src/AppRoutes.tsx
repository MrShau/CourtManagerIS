import React from 'react'
import Home from './pages/Home/Home'
import Notifications from './pages/Notifications/Notifications';
import Auth from './pages/Auth/Auth';
import Employees from './pages/Employees/Employees';
import Judges from './pages/Judges/Judges';
import Departments from './pages/Departments/Departments';
import Candidates from './pages/Candidates/Candidates';
import CourtCases from './pages/CourtCases/CourtCases';
import Events from './pages/Events/Events';
import Chat from './pages/Chat/Chat';

const AppRoutes = [{
    path: "/",
    element: <Home />
}, {
    path: '/notifications',
    element: <Notifications />
}, {
    path: '/auth',
    element: <Auth />
}, {
    path: '/employees',
    element: <Employees />
}, {
    path: '/judges',
    element: <Judges />
}, {
    path: '/departments',
    element: <Departments />
}, {
    path: '/candidates',
    element: <Candidates />
}, {
    path: '/courtcases',
    element: <CourtCases />
}, {
    path: '/events',
    element: <Events />
}, {
    path: '/chat', 
    element: <Chat />
}

];

export  default AppRoutes;