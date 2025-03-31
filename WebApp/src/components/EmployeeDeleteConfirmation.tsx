import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
} from "@mui/material";
import GetEmployeeDto from "../models/getEmployeeDto";
import { useMutation } from "@tanstack/react-query";
import DeleteEmployeeDto from "../models/deleteEmployeeDto";
import useEmployeeApi from "../api/useEmployeeApi";

interface EmployeeDeleteConfirmationProps {
  employee: GetEmployeeDto;
  onCancel: () => void;
  onDeleted: () => void;
}

const EmployeeDeleteConfirmation = (props: EmployeeDeleteConfirmationProps) => {
  const { deleteEmployee } = useEmployeeApi();

  const mutation = useMutation({
    mutationFn: (dto: DeleteEmployeeDto) => deleteEmployee(dto),
    onSuccess: () => {
      props.onDeleted();
      props.onCancel();
    },
  });

  const onDelete = () => {
    mutation.mutate({
      id: props.employee.id,
    });
  };

  return (
    <Dialog maxWidth="sm" fullWidth open>
      <DialogTitle>Delete Employee {props.employee.name}</DialogTitle>
      <DialogContent>
        Are you sure you want to delete {props.employee.name}?
      </DialogContent>
      <DialogActions>
        <Button
          color="secondary"
          onClick={props.onCancel}
          disabled={mutation.isPending}
        >
          No
        </Button>
        <Button color="error" onClick={onDelete} disabled={mutation.isPending}>
          Yes
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default EmployeeDeleteConfirmation;
