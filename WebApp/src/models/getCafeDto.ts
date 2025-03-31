export default interface GetCafeDto {
  id: string;
  name: string;
  description: string;
  logo?: string;
  location: string;
	employees: number;
}