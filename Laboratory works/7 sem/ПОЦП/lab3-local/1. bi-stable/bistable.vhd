library IEEE;
use IEEE.std_logic_1164.all;

entity Bistable is
    port (
        Q   : out std_logic;
        NQ  : out std_logic
    );
end Bistable;

architecture Behavioral of Bistable is

    constant s : std_logic := '1';

    signal q_reg  : std_logic := not s;
    signal nq_reg : std_logic := s;
begin
    process
    begin
        q_reg <= not nq_reg;
        nq_reg <= not q_reg;
        wait;
    end process;

    Q  <= q_reg;
    NQ <= nq_reg;
end Behavioral;
