import React, { useContext, useEffect, useState } from 'react';
import './App.css';

import { HashRouter, Routes, Route } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import EmployeeApi from './api/EmployeeApi';
import { Spinner } from 'react-bootstrap';
import { Context } from '.';

function App() {
  
  const [stores, setStores] = useState(useContext(Context));
  const [loaded, setLoaded] = useState<boolean>(false);

  useEffect(() => {
    if (window.location.hash !== '#/auth') {
      if (localStorage.getItem('token') == null) {
        stores?.user?.setIsAuth(false);
        window.location.hash = '#/auth';
        return;
      }
    }

    EmployeeApi.auth().then(res => {
      if (res == null) {
        localStorage.removeItem('token');
        stores?.user.setIsAuth(false);
        if (window.location.hash !== '#/auth')
          window.location.hash = '#/auth';
        setLoaded(true);
        return;
      }

      stores?.user.setIsAuth(true);
      stores?.user.setUser(res)
      setLoaded(true);
      if (window.location.hash === '#/auth')
        window.location.hash = '#/'
    });
  }, []);

  return (
    loaded ?
      <div className='h-100'>
        <HashRouter>
          <Routes>
            {AppRoutes.map((route, index) => <Route key={index} path={route.path} element={route.element} />)}
          </Routes>
        </HashRouter>
      </div> :
      <div className="w-100 vh-100 p-0 m-0 d-flex align-items-center justify-content-center">
        <Spinner animation="grow" className="bg-light" />
      </div>
  );
}

export default App;
