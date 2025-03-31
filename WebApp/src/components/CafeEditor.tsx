import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Stack,
  TextField,
} from "@mui/material";
import CreateCafeDto from "../models/createCafeDto";
import GetCafeDto from "../models/getCafeDto";
import UpdateCafeDto from "../models/updateCafeDto";
import * as z from "zod";
import { SubmitHandler, useForm, useWatch } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import useCafeApi from "../api/useCafeApi";
import { useMutation } from "@tanstack/react-query";
import imageToBase64 from "../utils/imageToBase64";

interface CafeEditorProps {
  value?: UpdateCafeDto;
  onCancel: () => void;
  onSaved: (created: GetCafeDto) => void;
}

const CafeEditor = (props: CafeEditorProps) => {
  const schema: z.ZodType<CreateCafeDto | UpdateCafeDto> = z
    .object({
      id: z.string(),
      name: z.string().nonempty(),
      description: z.string().nonempty(),
      logo: z.string().nonempty(),
      location: z.string().nonempty(),
    })
    .partial({
      ...(!props.value && { id: true }),
      logo: true,
    });

  const {
    register,
    formState: { errors },
    handleSubmit,
    setValue,
    control,
  } = useForm<CreateCafeDto | UpdateCafeDto>({
    resolver: zodResolver(schema),
    defaultValues: {
      id: props.value?.id,
      name: props.value?.name,
      description: props.value?.description,
      location: props.value?.location,
    },
  });

  const watchLogo = useWatch({
    control,
    name: "logo",
  });

  const { createCafe, updateCafe } = useCafeApi();
  const mutation = useMutation({
    mutationFn: (dto: CreateCafeDto | UpdateCafeDto) => {
      if (props.value) {
        return updateCafe(dto as UpdateCafeDto);
      }
      return createCafe(dto);
    },
    onSuccess: (data) => {
      props.onSaved(data);
      props.onCancel();
    },
  });

  const onSave: SubmitHandler<CreateCafeDto | UpdateCafeDto> = (data) => {
    mutation.mutate(data);
  };

  return (
    <Dialog maxWidth="sm" fullWidth open>
      <DialogTitle>
        {props.value ? "Update Cafe" : "Create a New Cafe"}
      </DialogTitle>
      <DialogContent>
        <Stack
          direction="column"
          spacing={2}
          mt={1}
          sx={{
            width: "100%",
          }}
          alignItems='center'
        >
          <TextField
            fullWidth
            label="Name"
            {...register("name")}
            error={!!errors.name}
            helperText={errors.name?.message}
            disabled={mutation.isPending}
          />
          <TextField
            fullWidth
            label="Location"
            {...register("location")}
            error={!!errors.location}
            helperText={errors.location?.message}
            disabled={mutation.isPending}
          />
          {watchLogo ? <img src={watchLogo} width={36} height={36} /> : null}
          <Button
            component="label"
            role={undefined}
            variant="contained"
            tabIndex={-1}
            disabled={mutation.isPending}
          >
            Select Logo
            <input
              type="file"
              accept="image/*"
              onChange={async (event) =>
                setValue(
                  "logo",
                  (await imageToBase64(event.target.files![0])) as string
                )
              }
              style={{ display: "none" }}
            />
          </Button>
          <TextField
            fullWidth
            label="Description"
            {...register("description")}
            error={!!errors.description}
            helperText={errors.description?.message}
            disabled={mutation.isPending}
          />
        </Stack>
      </DialogContent>
      <DialogActions>
        <Button
          color="secondary"
          onClick={props.onCancel}
          disabled={mutation.isPending}
        >
          Cancel
        </Button>
        <Button
          color="primary"
          onClick={handleSubmit(onSave)}
          disabled={mutation.isPending}
        >
          {mutation.isPending
            ? !props.value
              ? "Creating"
              : "Updating"
            : !props.value
            ? "Create"
            : "Update"}
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default CafeEditor;
