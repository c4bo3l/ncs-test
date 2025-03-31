/* eslint-disable react-hooks/exhaustive-deps */
import { useQuery } from "@tanstack/react-query";
import useCafeApi from "../api/useCafeApi";
import { Autocomplete, CircularProgress, TextField } from "@mui/material";
import { useEffect, useState } from "react";
import GetCafeDto from "../models/getCafeDto";

interface CafeSelectorProps {
  cafeId?: string;
  onChanged: (val: string | null) => void;
  disabled?: boolean;
  error?: boolean;
  errorText?: string;
}

const CafeSelector = (props: CafeSelectorProps) => {
  const { getCafes } = useCafeApi();
  const [value, setValue] = useState<GetCafeDto | null>(null);
  const { data, isLoading, isFetched } = useQuery({
    queryKey: ["cafes"],
    queryFn: () => getCafes(),
  });

  useEffect(() => {
    if (!isFetched || !props.cafeId || !data) {
      return;
    }
    setValue(data.find((x) => x.id == props.cafeId) || null);
  }, [isFetched]);

  useEffect(() => {
    props.onChanged(value?.id || null);
  }, [value]);

  return (
    <Autocomplete
      options={data || []}
      loading={isLoading}
      value={value}
      isOptionEqualToValue={(option, value) => option.id === value.id}
      getOptionLabel={(option) => option.name}
      fullWidth
      onChange={(_, val) => {
        setValue(val);
      }}
      renderInput={(params) => (
        <TextField
          {...params}
          label="Cafe"
          variant="outlined"
          slotProps={{
            input: {
              ...params.InputProps,
              endAdornment: (
                <>
                  {isLoading && <CircularProgress color="inherit" size={20} />}
                  {params.InputProps.endAdornment}
                </>
              ),
            },
          }}
          error={props.error}
          helperText={props.errorText}
        />
      )}
      disabled={props.disabled}
    />
  );
};

export default CafeSelector;
