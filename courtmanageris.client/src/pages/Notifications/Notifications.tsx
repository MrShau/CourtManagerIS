import React, { useEffect, useState } from "react";
import { Col, Container, Row } from "react-bootstrap";

import Sidebar from "../../components/Sidebar/Sidebar";
import EmployeeApi, { NotificationType } from "../../api/EmployeeApi";

function Notifications() {

    const [items, setItems] = useState<NotificationType[]>([]);

    useEffect(() => {
        EmployeeApi.getNotifications().then(res => setItems(res));
    }, []);

    return (
        <Row className='h-100 m-0'>
            <Col className="background-dark col-4 col-lg-3 col-xxl-2 p-0 m-0 vh-100"><Sidebar activePage="/notifications"/></Col>
            <Col className='background-dark col-8 col-lg-9 col-xxl-10 py-2 pe-2 py-md-3 pe-md-3 py-lg-4 pe-lg-4 m-0 p-0'>
                <main className='background-light p-2 p-lg-3 p-xl-4 rounded-2 h-100 w-100 overflow-auto'>
                    <Container className="p-3" fluid>
                        <h5 className="text-center mb-3">Мои уведомления</h5>
                        {items.map((item, index) => {
                            return(<div key={index} className="border p-3 rounded-2 mb-2">
                                <div>{item.title}</div>
                                <hr/>
                                <div>{item.content}</div>
                                <hr/>
                                <div>Отправитель: {item.senderFIO}</div>
                            </div>)
                        })}
                    </Container>
                </main>
            </Col>
        </Row>
    )
}

export default Notifications;