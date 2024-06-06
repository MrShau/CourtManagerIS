import React, { useEffect, useState } from "react";
import { Button, Col, Container, FloatingLabel, Form, Row, Tab, Tabs } from "react-bootstrap";
import Sidebar from "../../components/Sidebar/Sidebar";
import EventApi, { EventType } from "../../api/EventApi";

function Events() {

    const [addTitle, setAddTitle] = useState<string>('');
    const [addDescription, setAddDescription] = useState<string>('');
    const [addStartDate, setStartDate] = useState<Date | null>(null);
    const [addEndDate, setEndDate] = useState<Date | null>(null);

    const [aItems, setAItems] = useState<EventType[]>([]);
    const [cItems, setCItems] = useState<EventType[]>([]);

    useEffect(() => {
        EventApi.getActives().then(res => setAItems(res));
        EventApi.getCompleted().then(res => setCItems(res));
    });

    return (
        <>
            <Row className='h-100 m-0'>
                <Col className="background-dark col-4 col-lg-3 col-xxl-2 p-0 m-0 vh-100"><Sidebar activePage="/events" /></Col>
                <Col className='background-dark col-8 col-lg-9 col-xxl-10 py-2 pe-2 py-md-3 pe-md-3 py-lg-4 pe-lg-4 m-0 p-0 vh-100'>
                    <main className='background-light p-2 p-lg-3 p-xl-4 rounded-2 h-100 w-100 overflow-auto'>
                        <Tabs defaultActiveKey="actives">
                            <Tab title={"Активные"} eventKey={'actives'}>
                                <div className="my-3">
                                    {aItems.map((item, index) => {
                                        return (
                                            <div key={index} className="rounded-0 border p-3 mb-2">
                                                <div><span className="small text-secondary">Заголовок:</span>{item.title}</div>
                                                <hr/>
                                                <div><span className="small text-secondary">Описание:</span> {item.description}</div>
                                                <hr/>
                                                <div>
                                                <div><span className="small text-secondary">Дата начала:</span>{item.startDate}</div>
                                                <div><span className="small text-secondary">Дата окончания:</span>{item.endDate}</div>
                                                <div><span className="small text-secondary">Организатор:</span>{item.createrFIO}</div>
                                                </div>

                                            </div>
                                        )
                                    })}
                                </div>
                            </Tab>
                            <Tab title={"Завершенные"} eventKey={'completed'}>
                            <div className="my-3">
                                    {cItems.map((item, index) => {
                                        return (
                                            <div key={index} className="rounded-0 border p-3 mb-2">
                                                <div><span className="small text-secondary">Заголовок:</span>{item.title}</div>
                                                <hr/>
                                                <div><span className="small text-secondary">Описание:</span> {item.description}</div>
                                                <hr/>
                                                <div>
                                                <div><span className="small text-secondary">Дата начала:</span>{item.startDate}</div>
                                                <div><span className="small text-secondary">Дата окончания:</span>{item.endDate}</div>
                                                <div><span className="small text-secondary">Организатор:</span>{item.createrFIO}</div>
                                                </div>

                                            </div>
                                        )
                                    })}
                                </div>
                            </Tab>
                            <Tab title={"Добавить"} eventKey={'add'}>
                                <div className="d-flex align-items-center justify-content-center mt-5">
                                    <div style={{ width: "324px" }} className="mt-5">
                                        <h5>Добавление мероприятия</h5>
                                        <hr />
                                        <div>
                                            <FloatingLabel controlId="floatingLabelLogin" label="Заголовок">
                                                <Form.Control
                                                    className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                                    placeholder="Заголовок"
                                                    onChange={e => setAddTitle(e.target.value)}
                                                />
                                            </FloatingLabel>
                                            <Form.Control
                                                className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                                placeholder="Описание"
                                                as='textarea'
                                                rows={4}
                                                onChange={e => setAddDescription(e.target.value)}
                                                />
                                            <FloatingLabel controlId="floatingLabelLogin" label="Дата начала">
                                                <Form.Control
                                                    className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                                    placeholder="Дата начала"
                                                    type='datetime-local'
                                                    onChange={e => setStartDate(new Date(e.target.value))}
                                                />
                                            </FloatingLabel>

                                            <FloatingLabel controlId="floatingLabelLogin" label="Дата окончания">
                                                <Form.Control
                                                    className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                                    placeholder="Дата окончания"
                                                    type='datetime-local'
                                                    onChange={e => setEndDate(new Date(e.target.value))}
                                                />
                                            </FloatingLabel>
                                            <Button variant="dark" className="mt-3 w-100" onClick={() => {
                                                EventApi.add(addTitle, addDescription, addStartDate, addEndDate);
                                            }}>
                                                Добавить
                                            </Button>
                                        </div>
                                    </div>
                                </div>
                            </Tab>
                        </Tabs>
                    </main>
                </Col>
            </Row>
        </>
    )
}

export default Events