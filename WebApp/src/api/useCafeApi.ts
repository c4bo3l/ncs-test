import useAxios from "../hooks/useAxios";
import CreateCafeDto from "../models/createCafeDto";
import DeleteCafeDto from "../models/deleteCafeDto";
import GetCafeDto from "../models/getCafeDto";
import UpdateCafeDto from "../models/updateCafeDto";

const useCafeApi = () => {
  const { create: createAxios } = useAxios();
  
  const getCafes = async (location?: string) => {
    const ax = await createAxios();
    const response = await ax.get<GetCafeDto[]>('/api/cafes', {
      params: {
        location
      }
    });
    return response.data;
  };

  const createCafe = async (dto: CreateCafeDto) => {
    const ax = await createAxios();
    const response = await ax.post<GetCafeDto>('/api/cafe', dto);
    return response.data;
  };

  const updateCafe = async (dto: UpdateCafeDto) => {
    const ax = await createAxios();
    const response = await ax.put<GetCafeDto>('/api/cafe', dto);
    return response.data;
  };

  const deleteCafe = async (dto: DeleteCafeDto) => {
    const ax = await createAxios();
    await ax.delete('/api/cafe', {
      data: dto
    });
  };

  return {
    getCafes,
    createCafe,
    updateCafe,
    deleteCafe
  };
};

export default useCafeApi;
