import React, { useContext, useEffect, useState } from "react";

import { Row, Col, Stack, Button, Container } from "react-bootstrap";
import Sidebar from "../../components/Sidebar/Sidebar";

import './Home.css'
import EventApi, { EventType } from "../../api/EventApi";
import EmployeeApi from "../../api/EmployeeApi";
import { API_HOST } from "../../consts";
import { Context } from "../..";

function Home() {
    const [statistics, setStatistics] = useState([{
        value: 1,
        month: 'Янв',
    },
    {
        value: 1,
        month: 'Фев',
    },
    {
        value: 1,
        month: 'Мар',
    },
    {
        value: 1,
        month: 'Апр',
    },
    {
        value: 1,
        month: 'Май',
    },
    {
        value: 1,
        month: 'Июн',
    },
    {
        value: 1,
        month: 'Июл',
    },
    {
        value: 1,
        month: 'Авг',
    },
    {
        value: 1,
        month: 'Сен',
    },
    {
        value: 0,
        month: 'Окт',
    },
    {
        value: 0,
        month: 'Ноя',
    },
    {
        value: 0,
        month: 'Дек',
    }])
    const maxHeightActivitiesChart = 120;

    const [events, setEvents] = useState<EventType[]>([]);
    const [countEmployees, setCountEmployees] = useState<number>(0);

    useEffect(() => {
        EventApi.getActives(10).then(res => setEvents(res))
        EmployeeApi.getChart().then(res => {
            res.map((item, index) => {
                let newS = statistics.slice();
                newS[index].value = item;
                setStatistics(newS)
            })
        })
        EmployeeApi.getCount().then(res => setCountEmployees(res))
    }, [])



    let maxActivitiesValue: number = statistics[0].value;
    const stores = useContext(Context)
    statistics.map((item) => maxActivitiesValue = maxActivitiesValue < item.value ? item.value : maxActivitiesValue);
    if (maxActivitiesValue < 0) maxActivitiesValue = 1;

    return (
        <Row className='h-100 m-0'>
            <Col className="background-dark col-4 col-lg-3 col-xxl-2 p-0 m-0 vh-100"><Sidebar activePage="/" /></Col>
            <Col className='background-dark col-8 col-lg-9 col-xxl-10 py-2 pe-2 py-md-3 pe-md-3 py-lg-4 pe-lg-4 m-0 p-0'>
                <main className='background-light p-2 p-lg-3 p-xl-4 rounded-2 h-100 w-100 overflow-auto'>
                    <Container className="p-3" fluid>
                        <Stack direction="horizontal" className="d-flex justify-content-between align-items-start" gap={3} >

                            <div className="col-8 col-xl-6 rounded-4 h-100" style={{ height: '600px' }}>
                                <div className="rounded-4 border p-3 bg-body">
                                    <div className="activities-block">
                                        <div>
                                            <span className="d-block" style={{ fontSize: '18px' }}>Активность сотрудников</span>
                                            <span className="text-secondary small">Количество сеансов</span>
                                        </div>
                                        <div className="d-flex activities-chart align-items-end mt-4 overflow-hidden">
                                            {statistics.map((item, index) => {
                                                return <div key={index} className="chart-item-block overflow-hidden"><div className="text-center text-secondary small">{item.value}</div><div className="bg-primary cursor-pointer w-100" style={{ height: `${item.value / maxActivitiesValue * maxHeightActivitiesChart}px` }}></div><div className="text-center text-secondary small mt-2">{item.month}</div></div>
                                            })}
                                        </div>
                                    </div>
                                </div>
                                <div className="d-flex justify-content-between mt-4">
                                    <div>
                                        <div className="d-flex">
                                            <div style={{ height: '64px', width: '64px' }} className="rounded-5 border overflow-hidden">
                                                <img src={`${API_HOST}${stores?.user.user?.imagePath}`} style={{ height: '64px', width: '64px' }} />
                                            </div>
                                            <div className="ms-2">
                                                <div>{stores?.user.user?.firstName} {stores?.user.user?.lastName}</div>
                                                <div className="small text-secondary">#{stores?.user.user?.login}</div>
                                                <div className="small text-success">Онлайн</div>
                                            </div>
                                        </div>
                                        <div className="d-flex mt-3">
                                            <div style={{ height: '64px', width: '64px' }} className="rounded-5 border overflow-hidden bg-secondary">

                                            </div>
                                            <div className="ms-2">
                                                <div className="bg-secondary" style={{ color: 'transparent' }}>{stores?.user.user?.firstName} {stores?.user.user?.lastName}</div>
                                                <div className="small  bg-secondary" style={{ color: 'transparent' }}>#{stores?.user.user?.login}</div>
                                                <div className="small bg-secondary" style={{ color: 'transparent' }}>Онлайн</div>
                                            </div>
                                        </div>
                                        <div className="d-flex mt-3">
                                            <div style={{ height: '64px', width: '64px' }} className="rounded-5 border overflow-hidden bg-secondary">

                                            </div>
                                            <div className="ms-2">
                                                <div className="bg-secondary" style={{ color: 'transparent' }}>{stores?.user.user?.firstName} {stores?.user.user?.lastName}</div>
                                                <div className="small  bg-secondary" style={{ color: 'transparent' }}>#{stores?.user.user?.login}</div>
                                                <div className="small bg-secondary" style={{ color: 'transparent' }}>Онлайн</div>
                                            </div>
                                        </div>

                                    </div>
                                    <div className="ms-4">
                                        <div>Всего работников: {countEmployees}</div>
                                        <div>Судьей: </div>
                                        <div>Заблокированный: </div>
                                        <div>Среднее количество сеансов за день: </div>
                                        <div>Средняя длительность сеансов: </div>
                                    </div>
                                </div>
                            </div>
                            <div className="col-4 col-xl-5 bg-body rounded-4 border p-3" style={{ height: '600px' }}>
                                <h5 className="text-center">Мероприятия</h5>
                                <hr />
                                <div className="overflow-auto">
                                    {events.map((item, index) => {
                                        return (
                                            <div className="overflow-hidden mb-2 pb-2 border-bottom" key={index}>
                                                <div>{item.title}</div>
                                                <div className="small">{item.startDate} -- {item.endDate}</div>
                                            </div>
                                        )
                                    })}
                                </div>
                            </div>
                        </Stack>
                    </Container>
                </main>
            </Col>
        </Row>

    );
}

export default Home;