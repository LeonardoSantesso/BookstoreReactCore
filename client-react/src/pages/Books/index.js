import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from "react-router-dom";
import { FiPower, FiEdit, FiTrash2 } from "react-icons/fi";
import api from '../../services/api';

import "./styles.css";
import logoImage from '../../assets/logo.svg'

export default function Book(){
    const [booksList, setBooksList] = useState([]);
    const [page, setPage] = useState(1);

    const userName = localStorage.getItem('userName');    
    const navigate = useNavigate();

    const accessToken = localStorage.getItem("accessToken");
    const authorization = { headers: { Authorization: `Bearer ${accessToken}`}};

    useEffect(() => { 
        try {
            fetchMoreBooks();
        } catch (error) {
            alert('Creation failed: Try again!');
        } 
     }, [accessToken])


     async function fetchMoreBooks(){
        const response = await api.get(`api/books/asc/5/${page}`, authorization);
        setBooksList([...booksList, ... response.data.list]);
        setPage(page + 1);        
     }

    async function deleteBook(id) {
        try {
            await api.delete(`api/books/${id}`, authorization);
            setBooksList(booksList.filter(book => book.id !== id));    
        } catch (error) {
            alert('Delete failed: Try again!');
        }
    }
    
    async function logout() {
        try {
            await api.get('api/auth/revoke', authorization);

            localStorage.clear();
            navigate('/');
        } catch (error) {
            alert('Logout failed: Try again!');
        }
    }
    

    async function editBook(id){
        try {
            navigate(`/book/new/${id}`);
        } catch (error) {
            alert('Edit book failed: Try again!');
        }
    }

    return(
        <div className="book-container">
            <header>
                <img src={logoImage} alt="Book store"/>
                <span>Welcome, <strong>{userName.toUpperCase()}</strong>!</span>
                <Link className="button" to="/book/new/0">Add new book</Link>
                <button type="button">
                    <FiPower size={18} color="251FC5" onClick={logout} />
                </button>
            </header>
            <h1>Registered Books</h1>
            <ul>
                {booksList.map(book => (
                    <li key={book.id}>
                        <strong>Title:</strong>
                        <p>{book.title}</p>
                        <strong>Author:</strong>
                        <p>{book.author}</p>
                        <strong>Price</strong>
                        <p>{Intl.NumberFormat('en-us', {style: 'currency', currency: 'USD'}).format(book.price)}</p>
                        <strong>Release Date</strong>
                        <p>{Intl.DateTimeFormat('en-US').format(new Date(book.launchDate))}</p>

                        <button type="button" onClick={() => editBook(book.id) }>
                            <FiEdit size={20} color="251FC5"/>
                        </button>
                        <button onClick={() => deleteBook(book.id)} type="button">
                            <FiTrash2 size={20} color="251FC5"/>
                        </button>
                    </li>
                ))}                                
            </ul>
            <button className='button' onClick={fetchMoreBooks} type='button'>Load more</button>
        </div>
    );
}