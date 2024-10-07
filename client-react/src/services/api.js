import axios from "axios";

const api = axios.create({
      baseURL:'https://localhost:7239'
})

export default api;