import React, { useEffect, useRef, useState } from "react";
import { Button, Col, Container, FloatingLabel, Form, Modal, Row, Table } from "react-bootstrap";
import Sidebar from "../../components/Sidebar/Sidebar";
import EmployeeApi, { getPageItemType } from "../../api/EmployeeApi";
import { API_HOST, dateToString, toFormattedDate } from "../../consts";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faFilter, faPlus, faRefresh, faSearch } from "@fortawesome/free-solid-svg-icons";
import DepartmentApi, { getAllNamesItem } from "../../api/DepartmentApi";
import PositionApi from "../../api/PositionApi";

import * as XLSX from 'xlsx'
import axios from "axios";

function Employees() {

    const [items, setItems] = useState<getPageItemType[]>([]);

    const [showSendNotification, setShowSendNotification] = useState<boolean>(false);
    const [notificationTitle, setNotificationTitle] = useState<string>('')
    const [notificationContent, setNotificationContent] = useState<string>('')

    const [showAddModal, setShowAddModal] = useState<boolean>(false);
    const [showEditDepartmentModal, setShowEditDepartmentModal] = useState<boolean>(false);
    const [showEditPositionModal, setShowEditPositionModal] = useState<boolean>(false);
    const [showEditPasswordModal, setShowEditPasswordModal] = useState<boolean>(false);
    const [showFilterModal, setShowFilterModal] = useState<boolean>(false);

    const [departments, setDepartments] = useState<getAllNamesItem[]>([]);
    const [positions, setPositions] = useState<getAllNamesItem[]>([]);

    const [addLogin, setAddLogin] = useState<string>('');
    const [addPassword, setAddPassword] = useState<string>('');
    const [addFirstName, setAddFirstName] = useState<string>('');
    const [addLastName, setAddLastName] = useState<string>('');
    const [addPatronymic, setAddPatronymic] = useState<string>('');
    const [addGender, setAddGender] = useState<string>('-1');
    const [addEducationType, setAddEducationType] = useState<string>('-1');
    const [addSeriePassport, setAddSeriePassport] = useState<string>('');
    const [addNumberPassport, setAddNumberPassport] = useState<string>('');
    const [addDepartment, setAddDepartment] = useState<string>('-1');
    const [addPosition, setAddPosition] = useState<string>('-1');
    const [addDateOfBirth, setAddDateOfBirth] = useState<Date>(new Date());
    const [addError, setAddError] = useState<string>('');

    const [EditLogin, setEditLogin] = useState<string>('');
    const [EditPassword, setEditPassword] = useState<string>('');
    const [EditFirstName, setEditFirstName] = useState<string>('');
    const [EditLastName, setEditLastName] = useState<string>('');
    const [EditPatronymic, setEditPatronymic] = useState<string>('');
    const [EditGender, setEditGender] = useState<string>('-1');
    const [EditEducationType, setEditEducationType] = useState<string>('-1');
    const [EditSeriePassport, setEditSeriePassport] = useState<string>('');
    const [EditNumberPassport, setEditNumberPassport] = useState<string>('');
    const [EditDepartment, setEditDepartment] = useState<string>('-1');
    const [EditPosition, setEditPosition] = useState<string>('-1');
    const [EditDateOfBirth, setEditDateOfBirth] = useState<Date>(new Date());
    const [EditError, setEditError] = useState<string>('');

    const filterSortRef = useRef<HTMLSelectElement | null>(null);
    const filterEducationRef = useRef<HTMLSelectElement | null>(null);
    const filterDepartmentRef = useRef<HTMLSelectElement | null>(null);
    const filterGenderRef = useRef<HTMLSelectElement | null>(null);

    const [filterSort, setFilterSort] = useState<number>(0);
    const [filterEducation, setFilterEducation] = useState<number>(-1);
    const [filterDepartment, setFilterDepartment] = useState<number>(-1);
    const [filterGender, setFilterGender] = useState<number>(-1);

    const [searchLogin, setSearchLogin] = useState<string>('');

    const getItems = () => {
        EmployeeApi.getPage(6, 1, filterSort, filterEducation, filterGender, filterDepartment, searchLogin).then(res => setItems(res));
        DepartmentApi.getAllNames().then(res => setDepartments(res));
    }

    useEffect(() => {
        getItems();
    }, []);

    useEffect(() => {
        if (Number(addDepartment) > -1) {
            PositionApi.getAllNames(Number(addDepartment)).then(res => setPositions(res))
        }
        else
            setPositions([]);
    }, [addDepartment])

    useEffect(() => {
        if (Number(EditDepartment) > -1) {
            PositionApi.getAllNames(Number(EditDepartment)).then(res => setPositions(res))
        }
        else
            setPositions([]);
    }, [EditDepartment])

    const onAddEmployee = async () => {
        if (addLogin.length < 3)
            return setAddError('Минимальная длина логина: 3');
        if (addPassword.length < 7)
            return setAddError('Минимальная длина пароля: 7');
        if (addFirstName.length < 3)
            return setAddError('Введите имя !');
        if (addLastName.length < 3)
            return setAddError('Введите фамилию !');
        if (addPatronymic.length < 3)
            return setAddError('Введите отчество !');
        if (Number(addGender) < 0)
            return setAddError('Выберите гендер !');
        if (addSeriePassport.length !== 4)
            return setAddError('Введите серию паспорта');
        if (addNumberPassport.length !== 6)
            return setAddError('Введите номер паспорта !');
        if (Number(addEducationType) < 0)
            return setAddError('Выберите тип образования !');
        if (Number(addDepartment) < 0)
            return setAddError('Выберите отдел !');
        if (Number(addPosition) < 0)
            return setAddError('Выберите должность !');
        if (addDateOfBirth.getFullYear() > 2006)
            return setAddError('Укажите дату рождения !');
        setAddError('');

        setAddError(await EmployeeApi.signUp(addLogin, addPassword, addFirstName, addLastName, addPatronymic, Number(addGender), Number(addEducationType), addSeriePassport, addNumberPassport, Number(addDepartment), Number(addPosition), addDateOfBirth));
        if (addError === '') {
            setShowAddModal(false)
            getItems();
        }
    }

    const onEditEmployee = async () => {
        if (EditLogin.length < 3)
            return setEditError('Минимальная длина логина: 3');
        if (EditFirstName.length < 3)
            return setEditError('Введите имя !');
        if (EditLastName.length < 3)
            return setEditError('Введите фамилию !');
        if (EditPatronymic.length < 3)
            return setEditError('Введите отчество !');
        if (Number(EditGender) < 0)
            return setEditError('Выберите гендер !');
        if (EditSeriePassport.length !== 4)
            return setEditError('Введите серию паспорта');
        if (EditNumberPassport.length !== 6)
            return setEditError('Введите номер паспорта !');
        if (Number(EditEducationType) < 0)
            return setEditError('Выберите тип образования !');
        if (EditDateOfBirth.getFullYear() > 2006 || EditDateOfBirth.getFullYear() < 1900)
            return setEditError('Укажите дату рождения !');
        setEditError('');

        let res = await EmployeeApi.get(editId)
        if (res == null)
            return setEditError('Ошибка сервера');

        res.dateOfBirth = new Date(res.dateOfBirth);

        setEditError(await EmployeeApi.updateBaseInfo(
            editId,
            EditLogin !== res.login ? EditLogin : null,
            EditFirstName !== res.firstName ? EditFirstName : null,
            EditLastName !== res.lastName ? EditLastName : null,
            EditPatronymic !== res.patronymic ? EditPatronymic : null,
            Number(EditGender) !== res.gender ? Number(EditGender) : null,
            EditSeriePassport !== res.seriePassport ? EditSeriePassport : null,
            EditNumberPassport !== res.numberPassport ? EditNumberPassport : null,
            Number(EditEducationType) !== res.educationType ? Number(EditEducationType) : null,
            EditDateOfBirth.toLocaleDateString() !== res.dateOfBirth.toLocaleDateString() ? EditDateOfBirth : null
        ))
        if (EditError === '') {
            alert('Изменения были успешно применены. Обновите таблицу чтобы увидеть изменения.');
            setShowEditModal(false);
        }
    }

    const [showEditModal, setShowEditModal] = useState<boolean>(false);

    const [editId, setEditId] = useState<number>(0);

    useEffect(() => {

        if (editId == 0)
            return;

        EmployeeApi.get(editId).then(res => {
            if (res == null)
                return;
            setEditLogin(res.login);
            setEditFirstName(res.firstName);
            setEditLastName(res.lastName);
            setEditPatronymic(res.patronymic);
            setEditSeriePassport(res.seriePassport);
            setEditNumberPassport(res.numberPassport);
            setEditGender(res.gender.toString());
            setEditDateOfBirth(new Date(res.dateOfBirth));
            setEditEducationType(res.educationType.toString());
            setEditDepartment(res.departmentId.toString());
            setEditPosition(res.positionId.toString());


            PositionApi.getAllNames(Number(EditDepartment)).then(res => setPositions(res))
        });

    }, [editId]);

    return (
        <>

            <Row className='h-100 m-0' >
                <Col className="background-dark p-0 m-0 vh-100 col-4 col-lg-3 col-xxl-2"><Sidebar activePage="/employees" /></Col>
                <Col className='background-dark vh-100 col-8 col-lg-9 col-xxl-10 py-2 pe-2 py-md-3 pe-md-3 py-lg-4 pe-lg-4 m-0 p-0'>
                    <main className='background-light p-3 p-xl-4 rounded-2 h-100 w-100 overflow-hidden' style={{
                        display: 'flex',
                        flexFlow: 'column'
                    }}>
                        <div className="d-flex align-items-center justify-content-between" style={{ flex: '0 1 auto' }}>
                            <span className='d-block' style={{ fontSize: '18px' }}>Сотрудники</span>

                            <div className="d-flex">
                                <div className="d-flex search-block me-3 me-lg-4 me-xl-5">
                                    <Form.Control
                                        type="search"
                                        className="border-bottom rounded-0 border-dark background-transparent"
                                        placeholder="Введите логин"
                                        style={{ fontSize: '14px' }}
                                        onChange={e => {
                                            setSearchLogin(e.target.value)
                                        }}
                                    />
                                    <Button variant="outline-dark" className=" rounded-0 border-bottom border-dark" onClick={() => getItems()}><span className="d-block" style={{ rotate: '90deg' }}><FontAwesomeIcon icon={faSearch} /></span></Button>
                                </div>

                                <Button variant="outline-dark" className="border-0 rounded-0 me-0" onClick={() => setShowFilterModal(true)}><FontAwesomeIcon icon={faFilter} /></Button>
                                <Button variant="outline-dark" className="border-0 rounded-0 me-0" onClick={() => getItems()}><FontAwesomeIcon icon={faRefresh} fixedWidth /></Button>
                                <Button variant="dark" className="border-0 rounded-0" onClick={() => setShowAddModal(true)}><FontAwesomeIcon icon={faPlus} fixedWidth /></Button>
                            </div>
                        </div>
                        <div className="mt-4 mb-2 overflow-auto" style={{ flex: '1 1 0px' }}>
                            <Table className="small" striped bordered responsive>
                                <thead>
                                    <tr>
                                        <th>ID</th>
                                        <th>Логин</th>
                                        <th>ФИО</th>
                                        <th>Пол</th>
                                        <th>Дата рождения</th>
                                        <th>Отдел</th>
                                        <th>Должность</th>
                                        <th>Статус блокировки</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {items.map((item, index) => <tr key={index} className="cursor-pointer" onClick={() => { setEditId(item.id); setShowEditModal(true) }}><td>{item.id}</td><td>{item.login}</td><td>{`${item.lastName} ${item.firstName} ${item.patronymic}`}</td><td>{item.gender}</td><td>{(new Date(item.dateOfBirth)).toLocaleDateString()}</td><td>{item.department}</td><td>{item.position}</td><td className={`${item.isBlocked ? 'text-danger' : 'text-success'}`}>{item.isBlocked ? 'Заблокирован' : 'Не заблокирован'}</td></tr>)}
                                </tbody>
                            </Table>
                        </div>

                        <Button variant="success" className="w-100" onClick={() => {
                            try {
                               
                                        let wb = XLSX.utils.book_new();
                                        let ws = XLSX.utils.json_to_sheet(items);
                                        XLSX.utils.book_append_sheet(wb, ws, 'Сотрудники');
                                        XLSX.writeFile(wb, 'Employees.xlsx')
                                    
                                    .catch();
                            }
                            catch (ex) { }
                        }}>
                                        Экспортировать в EXCEL
                        </Button>
                    </main>
                </Col>
            </Row>

            <Modal scrollable show={showFilterModal} onHide={() => setShowFilterModal(false)} centered>
                <Modal.Header closeButton closeVariant="white" className="background-dark">
                    <span className="px-2">Фильтры</span>
                    <Button variant="outline-dark" className="border-0 rounded-0 text-light" onClick={() => {
                        if (filterSortRef.current != null) {
                            filterSortRef.current.value = '0'
                            setFilterSort(0);
                        }
                        if (filterEducationRef.current != null) {
                            filterEducationRef.current.value = '-1'
                            setFilterEducation(-1)
                        }
                        if (filterDepartmentRef.current != null) {
                            filterDepartmentRef.current.value = '-1'
                            setFilterDepartment(-1);
                        }
                        if (filterGenderRef.current != null) {
                            filterGenderRef.current.value = '-1'
                            setFilterGender(-1);
                        }
                    }}><FontAwesomeIcon icon={faRefresh} /></Button>
                </Modal.Header>
                <Modal.Body>
                    <div className="d-flex">
                        <Form.Select className="border-0 background-transparent rounded-0" ref={filterSortRef} value={filterSort} onChange={e => setFilterSort(Number(e.target.value))}>
                            <option value='0'>Сначала старые</option>
                            <option value='1'>Сначала новые</option>
                        </Form.Select>
                        <Button variant="outline-dark" className="rounded-0 border-0" title="Сбросить" onClick={() => {
                            if (filterSortRef.current != null) {
                                filterSortRef.current.value = '0'
                                setFilterSort(0);
                            }
                        }}><FontAwesomeIcon icon={faRefresh} /></Button>
                    </div>

                    <div className="d-flex mt-3">
                        <Form.Select className="border-0 background-transparent rounded-0" ref={filterEducationRef} value={filterEducation} onChange={e => setFilterEducation(Number(e.target.value))}>
                            <option value='-1'>Тип образования</option>
                            <option value='0'>Среднее профессиональное</option>
                            <option value='1'>Бакалавриат</option>
                            <option value='2'>Магистратура</option>
                            <option value='3'>Аспирантура</option>
                        </Form.Select>
                        <Button variant="outline-dark" className="rounded-0 border-0" title="Сбросить" onClick={() => {
                            if (filterEducationRef.current != null) {
                                filterEducationRef.current.value = '-1'
                                setFilterEducation(-1)
                            }
                        }}><FontAwesomeIcon icon={faRefresh} /></Button>
                    </div>

                    <div className="d-flex mt-3">
                        <Form.Select className="border-0 background-transparent rounded-0" ref={filterDepartmentRef} value={filterDepartment} onChange={e => setFilterDepartment(Number(e.target.value))}>
                            <option value='-1'>Отдел</option>
                            {departments.map((item, index) => <option value={`${item.id}`} key={index}>{item.name}</option>)}
                        </Form.Select>
                        <Button variant="outline-dark" className="rounded-0 border-0" title="Сбросить" onClick={() => {
                            if (filterDepartmentRef.current != null) {
                                filterDepartmentRef.current.value = '-1'
                                setFilterDepartment(-1);
                            }
                        }}><FontAwesomeIcon icon={faRefresh} /></Button>
                    </div>

                    <div className="d-flex mt-3">
                        <Form.Select className="border-0 background-transparent rounded-0" ref={filterGenderRef} value={filterGender} onChange={e => setFilterGender(Number(e.target.value))}>
                            <option value='-1'>Пол</option>
                            <option value='0'>Женский</option>
                            <option value='1'>Мужской</option>
                        </Form.Select>
                        <Button variant="outline-dark" className="rounded-0 border-0" title="Сбросить" onClick={() => {
                            if (filterGenderRef.current != null) {
                                filterGenderRef.current.value = '-1'
                                setFilterGender(-1);
                            }
                        }}><FontAwesomeIcon icon={faRefresh} /></Button>
                    </div>

                </Modal.Body>
                <Modal.Footer className="p-0 background-dark">
                    <Button variant="ouline-dark" className="text-light w-100 h-100 py-2 border-0" onClick={e => getItems()}>Применить</Button>
                </Modal.Footer>
            </Modal>

            <Modal scrollable show={showAddModal} onHide={() => { setShowAddModal(false); setEditId(-1) }} centered>
                <Modal.Header closeButton closeVariant="white" className="background-dark">
                    <Modal.Title className="fs-5 ps-5">Регистрация нового сотрудника</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div className="px-5">
                        <FloatingLabel controlId="floatingLabelLogin" label="Логин">
                            <Form.Control
                                className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                placeholder="Логин"
                                onChange={e => setAddLogin(e.target.value)}
                            />
                        </FloatingLabel>
                        <FloatingLabel controlId="floatingLabelPassword" label="Пароль">
                            <Form.Control
                                className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                placeholder="Пароль"
                                onChange={e => setAddPassword(e.target.value)}
                            />
                        </FloatingLabel>
                        <FloatingLabel controlId="floatingLabelName" label="Имя">
                            <Form.Control
                                className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                placeholder="Имя"
                                onChange={e => setAddFirstName(e.target.value)}
                            />
                        </FloatingLabel>
                        <FloatingLabel controlId="floatingLabelLastName" label="Фамилия">
                            <Form.Control
                                className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                placeholder="Фамилия"
                                onChange={e => setAddLastName(e.target.value)}
                            />
                        </FloatingLabel>
                        <FloatingLabel controlId="floatingLabelPatronymic" label="Отчество">
                            <Form.Control
                                className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                placeholder="Отчество"
                                onChange={e => setAddPatronymic(e.target.value)}
                            />
                        </FloatingLabel>
                        <FloatingLabel controlId="floatingLabelDateOfBirth" label="Дата рождения">
                            <Form.Control
                                className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                placeholder="Отчество"
                                type="date"
                                min="1940-01-01"
                                max="2006-12-31"
                                onChange={e => setAddDateOfBirth(new Date(e.target.value))}
                            />
                        </FloatingLabel>
                        <FloatingLabel controlId="floatingLabelSeriaPassport" label="Серия паспорта">
                            <Form.Control
                                className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                placeholder="Серия паспорта"
                                onChange={e => setAddSeriePassport(e.target.value)}
                                maxLength={4}
                            />
                        </FloatingLabel>
                        <FloatingLabel controlId="floatingLabelNumberPassport" label="Номер паспорта">
                            <Form.Control
                                className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                placeholder="Номер паспорта"
                                onChange={e => setAddNumberPassport(e.target.value)}
                                maxLength={6}
                            />
                        </FloatingLabel>
                        <Form.Select className="background-transparent border-0 border-bottom rounded-0 border-dark small py-3"
                            onChange={e => setAddGender(e.target.value)}>
                            <option value="-1">Пол</option>
                            <option value="0">Женский</option>
                            <option value="1">Мужской</option>
                        </Form.Select>
                        <Form.Select className="background-transparent border-0 border-bottom rounded-0 border-dark small py-3"
                            onChange={e => setAddEducationType(e.target.value)}>
                            <option value="-1">Тип образования</option>
                            <option value="0">Среднее профессиональное</option>
                            <option value="1">Бакалавр</option>
                            <option value="2">Магистр</option>
                            <option value="3">Аспирант</option>
                        </Form.Select>
                        <Form.Select className="background-transparent border-0 border-bottom rounded-0 border-dark small py-3"
                            onChange={e => setAddDepartment(e.target.value)}>
                            <option value="-1">Отдел</option>
                            {departments.map((item, index) => <option value={`${item.id}`} key={index}>{item.name}</option>)}
                        </Form.Select>
                        <Form.Select className="background-transparent border-0 border-bottom rounded-0 border-dark small py-3"
                            onChange={e => setAddPosition(e.target.value)}>
                            <option value="-1">Должность</option>
                            {positions.map((item, index) => <option value={`${item.id}`} key={index}>{item.name}</option>)}
                        </Form.Select>
                    </div>
                    <div className="px-5 my-2 text-danger small">{addError}</div>
                </Modal.Body>
                <Modal.Footer className="p-0 background-dark">
                    <Button variant="outline-dark" className="text-light w-100 rounded-0 py-2 h-100 border-0" onClick={onAddEmployee}>
                        Добавить
                    </Button>
                </Modal.Footer>
            </Modal>

            <Modal size="lg" scrollable show={showEditModal} onHide={() => setShowEditModal(false)} centered>
                <Modal.Header closeButton closeVariant="white" className="background-dark">
                    <Modal.Title className="ps-5 fs-5">Редактирования пользователя {editId}</Modal.Title>
                </Modal.Header>
                <Modal.Body >
                    <div className="d-flex justify-content-between">
                        <div className="col-7 px-2 px-md-3 px-lg-5">
                            <FloatingLabel controlId="floatingLabelLogin" label="Логин">
                                <Form.Control
                                    className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                    placeholder="Логин"
                                    onChange={e => setEditLogin(e.target.value)}
                                    value={EditLogin}
                                />
                            </FloatingLabel>
                            <FloatingLabel controlId="floatingLabelName" label="Имя">
                                <Form.Control
                                    className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                    placeholder="Имя"
                                    onChange={e => setEditFirstName(e.target.value)}
                                    value={EditFirstName}
                                />
                            </FloatingLabel>
                            <FloatingLabel controlId="floatingLabelLastName" label="Фамилия">
                                <Form.Control
                                    className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                    placeholder="Фамилия"
                                    onChange={e => setEditLastName(e.target.value)}
                                    value={EditLastName}
                                />
                            </FloatingLabel>
                            <FloatingLabel controlId="floatingLabelPatronymic" label="Отчество">
                                <Form.Control
                                    className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                    placeholder="Отчество"
                                    onChange={e => setEditPatronymic(e.target.value)}
                                    value={EditPatronymic}
                                />
                            </FloatingLabel>
                            <FloatingLabel controlId="floatingLabelDateOfBirth" label="Дата рождения">
                                <Form.Control
                                    className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                    placeholder="Дата рождения"
                                    type="date"
                                    min="1940-01-01"
                                    max="2006-12-31"
                                    onChange={e => setEditDateOfBirth(new Date(e.target.value))}
                                    value={toFormattedDate(EditDateOfBirth)}
                                />
                            </FloatingLabel>
                            <FloatingLabel controlId="floatingLabelSeriaPassport" label="Серия паспорта">
                                <Form.Control
                                    className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                    placeholder="Серия паспорта"
                                    onChange={e => setEditSeriePassport(e.target.value)}
                                    maxLength={4}
                                    value={EditSeriePassport}
                                />
                            </FloatingLabel>
                            <FloatingLabel controlId="floatingLabelNumberPassport" label="Номер паспорта">
                                <Form.Control
                                    className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                    placeholder="Номер паспорта"
                                    onChange={e => setEditNumberPassport(e.target.value)}
                                    maxLength={6}
                                    value={EditNumberPassport}
                                />
                            </FloatingLabel>
                            <Form.Select className="background-transparent border-0 border-bottom rounded-0 border-dark small py-3"
                                onChange={e => setEditGender(e.target.value)} value={EditGender}>
                                <option value="-1">Пол</option>
                                <option value="0">Женский</option>
                                <option value="1">Мужской</option>
                            </Form.Select>
                            <Form.Select className="background-transparent border-0 border-bottom rounded-0 border-dark small py-3"
                                onChange={e => setEditEducationType(e.target.value)} value={EditEducationType}>
                                <option value="-1">Тип образования</option>
                                <option value="0">Среднее профессиональное</option>
                                <option value="1">Бакалавр</option>
                                <option value="2">Магистр</option>
                                <option value="3">Аспирант</option>
                            </Form.Select>
                            <div className="my-2 text-danger small">{EditError}</div>

                        </div>
                        <div className="col-5 col-xl-4 me-lg-2">
                            <Button variant="outline-dark" className="px-lg-5 py-2 w-100 mt-2 rounded-0" onClick={async () => {
                                await EmployeeApi.block(editId, false);
                            }}>Разблокировать</Button>
                            <Button variant="outline-dark" className="px-lg-5 py-2 w-100 mt-2 rounded-0" onClick={async () => {
                                await EmployeeApi.block(editId, true);
                            }}>Заблокировать</Button>
                            <Button variant="outline-dark" className="px-lg-5 py-2 mt-2 w-100 rounded-0" onClick={() => {
                                setShowEditModal(false);
                                setShowEditDepartmentModal(true);
                            }}>Перевести в другой отдел</Button>
                            <Button variant="outline-dark" className="px-lg-5 py-2 mt-2 w-100 rounded-0" onClick={() => {
                                setShowEditModal(false);
                                setShowEditPositionModal(true);
                            }}>Изменить должность</Button>
                            <Button variant="outline-dark" className="px-lg-5 py-2 mt-2 w-100 rounded-0" onClick={() => {
                                setShowEditModal(false);
                                setShowSendNotification(true);
                            }}>Отправить уведомление</Button>
                            <Button variant="outline-dark" className="px-lg-5 py-2 mt-2 w-100 rounded-0" onClick={() => {
                                setShowEditModal(false);
                                setShowEditPasswordModal(true);
                            }}>Изменить пароль</Button>

                            <Button variant="outline-dark" className="px-lg-5 py-2 mt-2 w-100 rounded-0" onClick={async () => {

                                try {
                                    await axios.get(`${API_HOST}/api/employee/getactivityhistory?id=${editId}`, { headers: { Authorization: 'Bearer ' + localStorage.getItem('token') } })
                                        .then(res => {
                                            let wb = XLSX.utils.book_new();
                                            let ws = XLSX.utils.json_to_sheet(res.data);
                                            XLSX.utils.book_append_sheet(wb, ws, 'Активность');
                                            XLSX.writeFile(wb, 'Activities.xlsx')
                                        })
                                        .catch();
                                }
                                catch (ex) { }
                            }}>Экспортировать историю активности в виде Excel</Button>
                        </div>
                    </div>
                </Modal.Body>
                <Modal.Footer className="p-0 background-dark">
                    <Button variant="outline-dark" className="w-100 h-100 text-light rounded-0 py-2" onClick={onEditEmployee}>
                        Применить
                    </Button>
                </Modal.Footer>
            </Modal>

            <Modal show={showEditDepartmentModal} centered onHide={() => {
                setShowEditDepartmentModal(false)
                setShowEditModal(true);
            }}>
                <Modal.Header closeButton closeVariant="white" className="background-dark">
                    <Modal.Title className="ps-5 fs-5">Перевод в другой отдел</Modal.Title>
                </Modal.Header>
                <Modal.Body >
                    <div className="col-12 px-5">
                        <Form.Select className="background-transparent border-0 border-bottom rounded-0 border-dark small py-3"
                            onChange={e => setEditDepartment(e.target.value)} value={EditDepartment}>
                            <option value="-1">Отдел</option>
                            {departments.map((item, index) => <option value={`${item.id}`} key={index}>{item.name}</option>)}
                        </Form.Select>
                        <Form.Select className="background-transparent border-0 border-bottom rounded-0 border-dark small py-3"
                            onChange={e => setEditPosition(e.target.value)} value={EditPosition}>
                            <option value="-1">Должность</option>
                            {positions.map((item, index) => <option value={`${item.id}`} key={index}>{item.name}</option>)}
                        </Form.Select>
                        <div className="my-2 text-danger small">{EditError}</div>
                    </div>
                </Modal.Body>
                <Modal.Footer className="px-5 background-dark">
                    <Button variant="outline-dark" className="w-100 text-light rounded-0 py-2" onClick={async () => {
                        if (EditDepartment === '-1')
                            return setEditError('Выберите отдел');
                        if (EditPosition === '-1')
                            return setEditError('Выберите должность');
                        setEditError('');
                        setEditError(await EmployeeApi.updateEmployeeDepartment(editId, Number(EditPosition)));
                    }}>
                        Применить
                    </Button>
                </Modal.Footer>
            </Modal>

            <Modal show={showEditPositionModal} centered onHide={() => {
                setShowEditPositionModal(false)
                setShowEditModal(true);
            }}>
                <Modal.Header closeButton closeVariant="white" className="background-dark">
                    <Modal.Title className="ps-5 fs-5">Изменения должности</Modal.Title>
                </Modal.Header>
                <Modal.Body >
                    <div className="col-12 px-5">
                        <Form.Select className="background-transparent border-0 border-bottom rounded-0 border-dark small py-3"
                            onChange={e => setEditPosition(e.target.value)} value={EditPosition}>
                            <option value="-1">Должность</option>
                            {positions.map((item, index) => <option value={`${item.id}`} key={index}>{item.name}</option>)}
                        </Form.Select>
                        <div className="my-2 text-danger small">{EditError}</div>
                    </div>
                </Modal.Body>
                <Modal.Footer className="px-5 background-dark">
                    <Button variant="outline-dark" className="w-100 text-light rounded-0 py-2" onClick={async () => {
                        if (EditPosition === '-1')
                            return setEditError('Выберите должность');
                        setEditError('');
                        setEditError(await EmployeeApi.updateEmployeePosition(editId, Number(EditPosition)));
                    }}>
                        Применить
                    </Button>
                </Modal.Footer>
            </Modal>

            <Modal show={showEditPasswordModal} centered onHide={() => {
                setShowEditPasswordModal(false)
                setShowEditModal(true);
            }}>
                <Modal.Header closeButton closeVariant="white" className="background-dark">
                    <Modal.Title className="ps-5 fs-5">Изменения пароля</Modal.Title>
                </Modal.Header>
                <Modal.Body >
                    <div className="col-12 px-5">
                        <FloatingLabel controlId="floatingLabelLogin" label="Пароль">
                            <Form.Control
                                className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                placeholder="Пароль"
                                onChange={e => setEditPassword(e.target.value)}
                            />
                        </FloatingLabel>
                        <div className="my-2 text-danger small">{EditError}</div>
                    </div>
                </Modal.Body>
                <Modal.Footer className="px-5 background-dark">
                    <Button variant="outline-dark" className="w-100 text-light rounded-0 py-2" onClick={async () => {
                        if (EditPassword.length < 7)
                            return setEditError('Минимальная длина пароля: 7');
                        setEditError('');
                        await EmployeeApi.updatePassword(editId, EditPassword);
                    }}>
                        Применить
                    </Button>
                </Modal.Footer>
            </Modal>

            <Modal show={showSendNotification} centered onHide={() => {
                setShowSendNotification(false)
                setShowEditModal(true);
            }}>
                <Modal.Header closeButton closeVariant="white" className="background-dark">
                    <Modal.Title className="ps-5 fs-5">Изменения пароля</Modal.Title>
                </Modal.Header>
                <Modal.Body >
                    <div className="col-12 px-5">
                        <FloatingLabel controlId="floatingLabelLogin" label="Заголовок">
                            <Form.Control
                                className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                placeholder="Заголовок"
                                onChange={e => setNotificationTitle(e.target.value)}
                            />
                        </FloatingLabel>
                        <FloatingLabel controlId="floatingLabelLogin" label="Текст">
                            <Form.Control
                                className="background-transparent border-0 border-bottom rounded-0 border-dark small"
                                placeholder="Пароль"
                                onChange={e => setNotificationContent(e.target.value)}
                            />
                        </FloatingLabel>
                    </div>
                </Modal.Body>
                <Modal.Footer className="px-5 background-dark">
                    <Button variant="outline-dark" className="w-100 text-light rounded-0 py-2" onClick={async () => {
                        if (notificationTitle.length > 0 && notificationContent.length > 0)
                            await EmployeeApi.sendNotification(editId, notificationTitle, notificationContent);
                    }}>
                        Применить
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    )
}

export default Employees;