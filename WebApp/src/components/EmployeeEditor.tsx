import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControl,
  FormControlLabel,
  FormLabel,
  Radio,
  RadioGroup,
  Stack,
  TextField,
} from "@mui/material";
import CreateEmployeeDto from "../models/createEmployeeDto";
import GetEmployeeDto from "../models/getEmployeeDto";
import UpdateEmployeeDto from "../models/updateEmployeeDto";
import * as z from "zod";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import useEmployeeApi from "../api/useEmployeeApi";
import { useMutation } from "@tanstack/react-query";
import CafeSelector from "./CafeSelector";
import { DatePicker } from "@mui/x-date-pickers";
import moment from "moment";

interface EmployeeEditorProps {
  value?: UpdateEmployeeDto;
  onCancel: () => void;
  onSaved: (created: GetEmployeeDto) => void;
}

const EmployeeEditor = (props: EmployeeEditorProps) => {
  const schema: z.ZodType<CreateEmployeeDto | UpdateEmployeeDto> = z
    .object({
      id: z.string(),
      name: z.string().nonempty(),
      email: z.string().email().nonempty(),
      phone: z
        .string()
        .length(8)
        .regex(/^[89]\d{7}/, {
          message: "Phone must be start with either 9 or 8",
        }),
      gender: z.string().nonempty(),
      cafeId: z.string().nonempty(),
      startDate: z.date(),
    })
    .partial({
      ...(!props.value && { id: true }),
      cafeId: true,
      startDate: true,
    });

  const {
    register,
    formState: { errors },
    handleSubmit,
    setValue,
    control,
  } = useForm<CreateEmployeeDto | UpdateEmployeeDto>({
    resolver: zodResolver(schema),
    defaultValues: {
      id: props.value?.id,
      name: props.value?.name,
      email: props.value?.email,
      phone: props.value?.phone,
      gender: props.value?.gender || "M",
      cafeId: props.value?.cafeId,
      startDate: props.value?.startDate,
    },
  });

  const { createEmployee, updateEmployee } = useEmployeeApi();
  const mutation = useMutation({
    mutationFn: (dto: CreateEmployeeDto | UpdateEmployeeDto) => {
      if (props.value) {
        return updateEmployee(dto as UpdateEmployeeDto);
      }
      return createEmployee(dto);
    },
    onSuccess: (data) => {
      props.onSaved(data);
      props.onCancel();
    },
  });

  const onSave: SubmitHandler<CreateEmployeeDto | UpdateEmployeeDto> = (
    data
  ) => {
    mutation.mutate(data);
  };

  return (
    <Dialog maxWidth="sm" fullWidth open>
      <DialogTitle>
        {props.value ? "Update Employee" : "Create a New Employee"}
      </DialogTitle>
      <DialogContent>
        <Stack
          direction="column"
          spacing={2}
          mt={1}
          sx={{
            width: "100%",
          }}
          alignItems="center"
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
            label="Email"
            {...register("email")}
            error={!!errors.email}
            helperText={errors.email?.message}
            disabled={mutation.isPending}
            slotProps={{
              input: {
                type: "email",
              },
            }}
          />
          <TextField
            fullWidth
            label="Phone"
            {...register("phone")}
            error={!!errors.phone}
            helperText={errors.phone?.message}
            disabled={mutation.isPending}
          />
          <Controller
            control={control}
            name="gender"
            render={({ fieldState, field }) => (
              <FormControl
                error={!!fieldState.error}
                fullWidth
                disabled={mutation.isPending}
              >
                <FormLabel id="gender-selector">Gender</FormLabel>
                <RadioGroup
                  row
                  aria-labelledby="gender-selector"
                  name="gender-selector"
                  value={field.value}
                  onChange={(_, val) => setValue('gender', val)}
                >
                  <FormControlLabel
                    value="F"
                    control={<Radio />}
                    label="Female"
                  />
                  <FormControlLabel
                    value="M"
                    control={<Radio />}
                    label="Male"
                  />
                </RadioGroup>
              </FormControl>
            )}
          />
          <Controller
            control={control}
            name="cafeId"
            render={({ fieldState, field }) => (
              <CafeSelector
                cafeId={field.value}
                onChanged={(val) => {
                  setValue("cafeId", val || undefined);
                  setValue("startDate", val ? new Date() : undefined);
                }}
                disabled={mutation.isPending}
                error={!!fieldState.error}
                errorText={fieldState.error?.message}
              />
            )}
          />
          <Controller
            control={control}
            name="startDate"
            render={({ fieldState, field }) => (
              <DatePicker
                label="Start Date"
                format="DD MMM YYYY"
                value={field.value ? moment(field.value) : null}
                onChange={(val) =>
                  setValue("startDate", val ? val.toDate() : undefined)
                }
                disabled={mutation.isPending}
                slotProps={{
                  textField: {
                    error: !!fieldState.error,
                    helperText: fieldState.error?.message,
                    fullWidth: true,
                  },
                }}
              />
            )}
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

export default EmployeeEditor;
