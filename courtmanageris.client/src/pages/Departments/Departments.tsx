import React, { useEffect, useState } from "react"
import { Button, Col, FloatingLabel, Form, Modal, Row, Table } from "react-bootstrap";
import Sidebar from "../../components/Sidebar/Sidebar";
import { faPlus, faRefresh } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import DepartmentApi, { departmentType, getPageDepartamentItem } from "../../api/DepartmentApi";

import * as XLSX from 'xlsx'

function Departments() {

    const [showAddModal, setShowAddModal] = useState<boolean>(false);    
    const [addName, setAddName] = useState<string>('');
    const [addDescription, setAddDescription] = useState<string>('');
    const [addError, setAddError] = useState<string>('');

    const [items, setItems] = useState<getPageDepartamentItem[]>([]);

    const [showEditModal, setShowEditModal] = useState<boolean>(false);
    const [editName, setEditName] = useState<string>('');
    const [editDescription, setEditDescription] = useState<string>('');
    const [editError, setEditError] = useState<string>('');
    const [editId, setEditId] = useState<number>(-1);

    const [editData, setEditData] = useState<departmentType | null>(null);

    const [showSetSupervisorModal, setShowSetSupervisorModal] = useState(false);
    const [supervisorLogin, setSupervisorLogin] = useState<string>('');
    const [supervisorError, setSupervisorError] = useState<string>('');

    let getItems = () => {
        DepartmentApi.getList(12, 1).then(res => setItems(res)).catch();
    }

    useEffect(() => {
        getItems();
    }, [])

    return(
        <>
        <Row className='h-100 m-0' >
                <Col className="background-dark p-0 m-0 vh-100 col-4 col-lg-3 col-xxl-2"><Sidebar activePage="/departments" /></Col>
                <Col className='background-dark vh-100 col-8 col-lg-9 col-xxl-10 py-2 pe-2 py-md-3 pe-md-3 py-lg-4 pe-lg-4 m-0 p-0'>
                    <main className='background-light p-3 p-xl-4 rounded-2 h-100 w-100 overflow-hidden' style={{
                        display: 'flex',
                        flexFlow: 'column'
                    }}>
                        <div className="d-flex align-items-center justify-content-between" style={{ flex: '0 1 auto' }}>
                            <span className='d-block' style={{ fontSize: '18px' }}>Отделы</span>

                            <div className="d-flex">
                                <Button variant="outline-dark" className="border-0 rounded-0 me-0" onClick={() => getItems()}><FontAwesomeIcon icon={faRefresh} fixedWidth /></Button>
                                <Button variant="dark" className="border-0 rounded-0" onClick={() => setShowAddModal(true)}><FontAwesomeIcon icon={faPlus} fixedWidth /></Button>
                            </div>
                        </div>
                        <div className="mt-4 mb-2 overflow-auto" style={{ flex: '1 1 0px' }}>
                            <Table className="small" striped bordered responsive>
                                <thead>
                                    <tr>
                                        <th>ID</th>
                                        <th>Наименование</th>
                                        <th>Описание</th>
                                        <th>Кол-во должностей</th>
                                        <th>Должности</th>
                                        <th>Кол-во сотрудников</th>
                                        <th>Руководитель</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {items.map((item, index) => <tr key={index} className="cursor-pointer" onClick={() => {
                                        setEditId(item.id); 
                                        setShowEditModal(true)
                                        ;
                                        }}><td>{item.id}</td><td>{item.name}</td><td>{item.description}</td><td>{item.positionsCount}</td><td>{item.positions}</td><td>{item.employeesCount}</td><td>{item.supervisor}</td></tr>)}
                                </tbody>
                            </Table>
                        </div>
                        <Button variant="success" className="w-100" onClick={() => {
                            try {
                               
                                        let wb = XLSX.utils.book_new();
                                        let ws = XLSX.utils.json_to_sheet(items);
                                        XLSX.utils.book_append_sheet(wb, ws, 'Судьи');
                                        XLSX.writeFile(wb, 'Judges.xlsx')
                                    
                                    .catch();
                            }
                            catch (ex) { }
                        }}>
                                        Экспортировать в EXCEL
                        </Button>
                    </main>
                </Col>
            </Row>
            
            <Modal scrollable show={showAddModal} onHide={() => { setShowAddModal(false) }} centered>
                <Modal.Header closeButton closeVariant="white" className="background-dark">
                    <Modal.Title className="fs-5 ps-5">Создание нового отдела</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div className="px-5">
                        <FloatingLabel controlId="floatingLabelLogin" label="Наименование">
                            <Form.Control
                                className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                placeholder="Наименование"
                                onChange={e => setAddName(e.target.value)}
                            />
                        </FloatingLabel>

                            <Form.Control
                                className="background-transparent border-0 border-bottom rounded-0 border-dark small mt-3"
                                placeholder="Описание"
                                as="textarea"
                                rows={3}
                                onChange={e => setAddDescription(e.target.value)}
                            />

                    </div>
                    <div className="px-5 my-2 text-danger small">{addError}</div>
                </Modal.Body>
                <Modal.Footer className="p-0 background-dark">
                    <Button variant="outline-dark" className="text-light w-100 rounded-0 py-2 h-100 border-0" onClick={async () => {
                        if (addName.length < 2)
                            return setAddError('Слишком короткое название')
                        setAddError(await DepartmentApi.add(addName, addDescription))
                    }}>
                        Добавить
                    </Button>
                </Modal.Footer>
            </Modal>

            <Modal size="lg" scrollable show={showEditModal} onShow={() => DepartmentApi.get(editId).then(res => {setEditName(res?.name ?? ''); setEditDescription(res?.description ?? '')}).catch()} onHide={() => { setShowEditModal(false) }} centered>
                <Modal.Header closeButton closeVariant="white" className="background-dark">
                    <Modal.Title className="fs-5 ps-5">Редактирование отдела</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                <div className="d-flex justify-content-between">
                        <div className="col-7 px-2 px-md-3 px-lg-5">
                        <FloatingLabel controlId="floatingLabelLogin" label="Наименование">
                            <Form.Control
                                className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                placeholder="Наименование"
                                value={editName}
                                onChange={e => setEditName(e.target.value)}
                            />
                        </FloatingLabel>

                            <Form.Control
                                className="background-transparent border-0 border-bottom rounded-0 border-dark small mt-3"
                                placeholder="Описание"
                                as="textarea"
                                defaultValue={editDescription}
                                rows={3}
                                onChange={e => setEditDescription(e.target.value)}
                            />
                        </div>
                        <div className="col-5 col-xl-4 me-lg-2">
                            <Button variant="outline-dark" className="rounded-0 w-100" onClick={() => setShowSetSupervisorModal(true)}>Поставить руководителя</Button>
                            <Button variant="outline-dark" className="rounded-0 w-100 mt-2"
                                onClick={() => {
                                    DepartmentApi.deleteSupervisor(editId).then(() => {setShowEditModal(false); getItems()})
                                }}
                            >Убрать руководителя</Button>
                            <Button variant="outline-dark" className="rounded-0 w-100 mt-2">Должности</Button>
                            <Button variant="outline-dark" className="rounded-0 w-100 mt-2">Удалить</Button>
                        </div>
                    </div>
                    <div className="px-5 my-2 text-danger small">{editError}</div>
                </Modal.Body>
                <Modal.Footer className="p-0 background-dark">
                    <Button variant="outline-dark" className="text-light w-100 rounded-0 py-2 h-100 border-0" onClick={async () => {
                        if (editName.length < 2)
                            return setEditError('Слишком короткое название')
                        setEditError(await DepartmentApi.update(editId, editName, editDescription))
                    }}>
                        Применить
                    </Button>
                </Modal.Footer>
            </Modal>

            <Modal scrollable show={showSetSupervisorModal} onShow={() => setShowEditModal(false)} onHide={() => { setShowSetSupervisorModal(false) }} centered>
                <Modal.Header closeButton closeVariant="white" className="background-dark">
                    <Modal.Title className="fs-5 ps-5">Поставить руководителя</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                <div>
                        <FloatingLabel controlId="floatingLabelLogin" label="Логин руководителя">
                            <Form.Control
                                className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                placeholder="Логин руководителя"
                                onChange={e => setSupervisorLogin(e.target.value)}
                            />
                        </FloatingLabel>
                    </div>
                    <div className="px-4 my-2 text-danger small">{supervisorError}</div>
                </Modal.Body>
                <Modal.Footer className="p-0 background-dark">
                    <Button variant="outline-dark" className="text-light w-100 rounded-0 py-2 h-100 border-0" onClick={async () => {
                        if (supervisorLogin.length < 2)
                            return setSupervisorError('Слишком короткое название')
                        setSupervisorError(await DepartmentApi.setSupervisor(editId, supervisorLogin))
                    }}>
                        Поставить
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    )

}

export default Departments