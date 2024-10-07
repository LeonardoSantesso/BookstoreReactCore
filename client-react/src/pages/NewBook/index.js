import React, { useState, useEffect } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { FiArrowLeft } from "react-icons/fi";
import api from '../../services/api';

import "./styles.css";
import logoImage from '../../assets/logo.svg'

export default function NewBook(){
    const [author, setAuthor] = useState('');
    const [title, setTitle] = useState('');
    const [lauchDate, setLauchDate] = useState('');
    const [price, setPrice] = useState('');
    const navigate = useNavigate();
    const { bookId } = useParams();
    
    const accessToken = localStorage.getItem("accessToken");
    const authorization = { headers: { Authorization: `Bearer ${accessToken}`}};

    useEffect(() => {
        if (bookId !== '0') {
            loadBook();
        }
    }, [bookId]);
    
    async function loadBook(){
        try {
            const response = await api.get(`api/books/${bookId}`, authorization);            
            let adjustedDate = response.data.launchDate.split('T', 10)[0];
            setAuthor(response.data.author);
            setTitle(response.data.title);
            setLauchDate(adjustedDate);
            setPrice(response.data.price);            
        } catch (error) {
            alert('Error recovering book: Try again!');
            navigate('/books');
        }
    }
    async function saveOrUpdate(e){
        e.preventDefault();
        const data = {
            author, 
            title,
            lauchDate,
            price
        };
        
        try {
            if(bookId !== '0' ){
                await api.put(`api/books/${bookId}`, data, authorization);        
            }else{
                await api.post('api/books', data, authorization);        
            }            
            navigate('/books');
        } catch (error) {
            alert('Creation failed: Try again!');
        }        
    }

    return(
        <div className="new-book-container">
            <div className="content">
                <section className="form">
                    <img src={logoImage} alt="New Book"></img>
                    <h1>{ bookId === '0' ? 'Add New ' : 'Update ' } Book</h1>
                    <p>Enter the book information and click on { bookId === '0' ? 'Add' : 'Update ' }</p>
                    <Link className="back-link" to="/books">
                        <FiArrowLeft size={16} color="251FC5"/> Back to book list                   
                    </Link>
                </section>
                <form onSubmit={saveOrUpdate}>
                    <input placeholder="Title" value={title} onChange={e => setTitle(e.target.value)}/>
                    <input placeholder="Author" value={author} onChange={e => setAuthor(e.target.value)}/>
                    <input type="date" value={lauchDate} onChange={e => setLauchDate(e.target.value)}/>
                    <input placeholder="Price" value={price} onChange={e => setPrice(e.target.value)}/>
                    <button type="submit" className="button">{ bookId === '0' ? 'Add' : 'Update' }</button>
                </form>
            </div>
        </div>
    );
}