import React, { useState } from "react"
import { Button, Container, Form } from "react-bootstrap";

import './Auth.css'
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faLock, faUser } from "@fortawesome/free-solid-svg-icons";
import EmployeeApi from "../../api/EmployeeApi";

function Auth() {
    
    const [login, setLogin] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [error, setError] = useState<string>('');


    const onSignIn = (e: any) => {

        if (login.length < 3 || login.length > 70)
            return setError('Логин не найден');
        if (password.length < 7)
            return setError('Минимальная длина пароля: 7 символов');

         EmployeeApi.signIn(login, password).then(res => setError(res)).catch()
    }

    return (
        <Container className="d-flex align-items-center vh-100">
            <div className="mx-auto background-light p-4 rounded-4 text-center" style={{width: '330px'}}>
                <div>
                    <img src="images/auth_avatar.png" width={100} alt="Авторизация" />
                    <h5 className="mt-3">Авторизация</h5>
                </div>
                <div className="mt-4">
                    <div className="position-relative">
                        <div className="position-absolute p-2 d-flex align-items-center h-100 background-dark"><FontAwesomeIcon icon={faUser} fixedWidth/></div>
                        <Form.Control
                            type="name"
                            className="py-2 background-transparent border-0 border-bottom rounded-0 border-dark"
                            placeholder="Логин"
                            style={{paddingLeft: '45px', fontSize: '14px'}}
                            onChange={e => setLogin(e.target.value)}
                        />
                    </div>
                    <div className="position-relative mt-4">
                        <div className="position-absolute p-2 d-flex align-items-center h-100 background-dark"><FontAwesomeIcon icon={faLock} fixedWidth/></div>
                        <Form.Control
                            type="password"
                            className="py-2 background-transparent border-0 border-bottom rounded-0 border-dark"
                            placeholder="Пароль"
                            style={{paddingLeft: '45px', fontSize: '14px'}}
                            onChange={e => setPassword(e.target.value)}
                        />
                    </div>
                    <div className="my-3 text-start small text-danger">{error}</div>
                    <Button variant="dark" className="w-100" onClick={onSignIn}>Войти</Button>
                    <div className="mt-2 small"><span className="cursor-pointer" onClick={e => {alert('Если вы забыли пароль, то обратитесь к администратору')}}>Забыли пароль ?</span></div>
                </div>
            </div>
        </Container>
    )
}

export default Auth;