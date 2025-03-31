import axios from 'axios';
import { API_BASE_URL } from '../shared/links';

const useAxios = () => {
  const create = async () => {
    return axios.create({
      baseURL: API_BASE_URL,
    });
  };

  return {
    create
  };
};

export default useAxios;
