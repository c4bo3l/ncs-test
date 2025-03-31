import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
} from "@mui/material";
import GetCafeDto from "../models/getCafeDto";
import { useMutation } from "@tanstack/react-query";
import DeleteCafeDto from "../models/deleteCafeDto";
import useCafeApi from "../api/useCafeApi";

interface CafeDeleteConfirmationProps {
  cafe: GetCafeDto;
  onCancel: () => void;
  onDeleted: () => void;
}

const CafeDeleteConfirmation = (props: CafeDeleteConfirmationProps) => {
  const { deleteCafe } = useCafeApi();

  const mutation = useMutation({
    mutationFn: (dto: DeleteCafeDto) => deleteCafe(dto),
    onSuccess: () => {
      props.onDeleted();
      props.onCancel();
    },
  });

  const onDelete = () => {
    mutation.mutate({
      id: props.cafe.id,
    });
  };

  return (
    <Dialog maxWidth="sm" fullWidth open>
      <DialogTitle>Delete Cafe {props.cafe.name}</DialogTitle>
      <DialogContent>
        Are you sure you want to delete {props.cafe.name}?
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

export default CafeDeleteConfirmation;
