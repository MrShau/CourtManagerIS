import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';

import 'bootstrap/dist/css/bootstrap.min.css';
import { createContext } from 'react';
import UserStore from './stores/UserStore';
import EmployeeApi from './api/EmployeeApi';

export const Context = createContext<{ user: UserStore } | null>(null);


document.onvisibilitychange = () => {
  EmployeeApi.activityEnd();
}

window.onbeforeunload = () => {
  EmployeeApi.activityEnd();
}

/*ReactDOM.createRoot(document.getElementById('root')!).render(
  <Context.Provider value={{
    user: new UserStore()
  }}>
    <React.StrictMode>
      <App />
    </React.StrictMode>
  </Context.Provider>
);*/


let container : any = null;

document.addEventListener('DOMContentLoaded', function(event) {
  if (!container) {
    container = document.getElementById('root') as HTMLElement;
    const root = ReactDOM.createRoot(container)
    root.render(
      <Context.Provider value={{
        user: new UserStore()
      }}>
        <React.StrictMode>
          <App />
        </React.StrictMode>
      </Context.Provider>
    );
  }
});
